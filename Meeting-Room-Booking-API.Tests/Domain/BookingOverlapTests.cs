using Meeting_Room_Booking_API.Domain.Entities;
using Xunit;

namespace Meeting_Room_Booking_API.Tests.Domain;

public class BookingOverlapTests
{
    private readonly Room _room;
    private readonly Guid _roomId;

    // Baseline booking: 10:00 – 11:00 today
    private readonly DateTime _baseStart;
    private readonly DateTime _baseEnd;

    public BookingOverlapTests()
    {
        _room = new Room("Board Room", "Floor 3", 8);
        _roomId = _room.Id;

        _baseStart = DateTime.Today.AddDays(1).AddHours(10); // tomorrow 10:00
        _baseEnd = DateTime.Today.AddDays(1).AddHours(11);   // tomorrow 11:00

        // Seed one existing booking
        var existing = new Booking(_roomId, "Alice", _baseStart, _baseEnd);
        _room.AddBooking(existing);
    }

    // ─── SHOULD SUCCEED (no conflict) ──────────────────────────────────────────

    [Fact]
    public void AddBooking_Succeeds_WhenNewBookingStartsExactlyWhenExistingEnds()
    {
        // [10:00–11:00] existing, [11:00–12:00] new  →  no overlap (half-open intervals)
        var booking = new Booking(_roomId, "Bob", _baseEnd, _baseEnd.AddHours(1));
        _room.AddBooking(booking);

        Assert.Equal(2, _room.Bookings.Count);
    }

    [Fact]
    public void AddBooking_Succeeds_WhenNewBookingEndsExactlyWhenExistingStarts()
    {
        // [09:00–10:00] new, [10:00–11:00] existing  →  no overlap
        var booking = new Booking(_roomId, "Bob", _baseStart.AddHours(-1), _baseStart);
        _room.AddBooking(booking);

        Assert.Equal(2, _room.Bookings.Count);
    }

    [Fact]
    public void AddBooking_Succeeds_WhenNewBookingIsEntirelyBefore()
    {
        // [07:00–08:00] new, [10:00–11:00] existing  →  no overlap
        var booking = new Booking(_roomId, "Bob", _baseStart.AddHours(-3), _baseStart.AddHours(-2));
        _room.AddBooking(booking);

        Assert.Equal(2, _room.Bookings.Count);
    }

    [Fact]
    public void AddBooking_Succeeds_WhenNewBookingIsEntirelyAfter()
    {
        // [10:00–11:00] existing, [14:00–15:00] new  →  no overlap
        var booking = new Booking(_roomId, "Bob", _baseEnd.AddHours(3), _baseEnd.AddHours(4));
        _room.AddBooking(booking);

        Assert.Equal(2, _room.Bookings.Count);
    }

    [Fact]
    public void AddBooking_Succeeds_WhenRoomHasNoExistingBookings()
    {
        var emptyRoom = new Room("Empty Room", "Floor 1", 4);
        var booking = new Booking(emptyRoom.Id, "Bob", _baseStart, _baseEnd);
        emptyRoom.AddBooking(booking);

        Assert.Single(emptyRoom.Bookings);
    }

    // ─── SHOULD FAIL (conflict) ────────────────────────────────────────────────

    [Fact]
    public void AddBooking_Throws_WhenNewBookingCompletelySwallowsExisting()
    {
        // [09:00–12:00] new swallows [10:00–11:00] existing
        var booking = new Booking(_roomId, "Bob", _baseStart.AddHours(-1), _baseEnd.AddHours(1));

        Assert.Throws<InvalidOperationException>(() => _room.AddBooking(booking));
    }

    [Fact]
    public void AddBooking_Throws_WhenNewBookingIsCompletelyInsideExisting()
    {
        // [10:15–10:45] new inside [10:00–11:00] existing
        var booking = new Booking(_roomId, "Bob", _baseStart.AddMinutes(15), _baseEnd.AddMinutes(-15));

        Assert.Throws<InvalidOperationException>(() => _room.AddBooking(booking));
    }

    [Fact]
    public void AddBooking_Throws_WhenNewBookingOverlapsStartOfExisting()
    {
        // [09:30–10:30] new overlaps start of [10:00–11:00] existing
        var booking = new Booking(_roomId, "Bob", _baseStart.AddMinutes(-30), _baseStart.AddMinutes(30));

        Assert.Throws<InvalidOperationException>(() => _room.AddBooking(booking));
    }

    [Fact]
    public void AddBooking_Throws_WhenNewBookingOverlapsEndOfExisting()
    {
        // [10:30–11:30] new overlaps end of [10:00–11:00] existing
        var booking = new Booking(_roomId, "Bob", _baseStart.AddMinutes(30), _baseEnd.AddMinutes(30));

        Assert.Throws<InvalidOperationException>(() => _room.AddBooking(booking));
    }

    [Fact]
    public void AddBooking_Throws_WhenNewBookingIsExactSameSlot()
    {
        // Identical time range — [10:00–11:00] new == [10:00–11:00] existing
        var booking = new Booking(_roomId, "Bob", _baseStart, _baseEnd);

        Assert.Throws<InvalidOperationException>(() => _room.AddBooking(booking));
    }

    [Fact]
    public void AddBooking_Throws_WhenNewBookingOverlapsByOneMinute()
    {
        // [10:59–12:00] new overlaps [10:00–11:00] existing by 1 minute
        var booking = new Booking(_roomId, "Bob", _baseEnd.AddMinutes(-1), _baseEnd.AddHours(1));

        Assert.Throws<InvalidOperationException>(() => _room.AddBooking(booking));
    }

    // ─── HasConflict direct checks ─────────────────────────────────────────────

    [Fact]
    public void HasConflict_ReturnsFalse_WhenNoBookingsExist()
    {
        var emptyRoom = new Room("Empty", "N/A", 2);
        Assert.False(emptyRoom.HasConflict(_baseStart, _baseEnd));
    }

    [Fact]
    public void HasConflict_ReturnsFalse_ForAdjacentSlotAfter()
    {
        Assert.False(_room.HasConflict(_baseEnd, _baseEnd.AddHours(1)));
    }

    [Fact]
    public void HasConflict_ReturnsTrue_ForOverlappingSlot()
    {
        Assert.True(_room.HasConflict(_baseStart.AddMinutes(30), _baseEnd.AddMinutes(30)));
    }

    // ─── Booking constructor validation ────────────────────────────────────────

    [Fact]
    public void BookingConstructor_Throws_WhenEndTimeEqualsStartTime()
    {
        var time = DateTime.Today.AddDays(1).AddHours(10);

        Assert.Throws<ArgumentException>(() => new Booking(_roomId, "Bob", time, time));
    }

    [Fact]
    public void BookingConstructor_Throws_WhenEndTimeIsBeforeStartTime()
    {
        var start = DateTime.Today.AddDays(1).AddHours(11);
        var end = DateTime.Today.AddDays(1).AddHours(10);

        Assert.Throws<ArgumentException>(() => new Booking(_roomId, "Bob", start, end));
    }

    [Fact]
    public void BookingConstructor_Throws_WhenBookedByIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new Booking(_roomId, "", _baseStart, _baseEnd));
    }

    [Fact]
    public void BookingConstructor_Throws_WhenRoomIdIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new Booking(Guid.Empty, "Bob", _baseStart, _baseEnd));
    }
}
