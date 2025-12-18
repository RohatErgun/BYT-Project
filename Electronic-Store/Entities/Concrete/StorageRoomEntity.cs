using System;
using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.Concrete;

namespace Electronic_Store.Entities.Concrete 
{
    public class StorageRoomEntity : BaseEntity
    {
        
        private WarehouseEntity _warehouse;
        
        
        private string _roomName;
        private double _squareFootage;

        public WarehouseEntity Warehouse => _warehouse;

        public string RoomName
        {
            get => _roomName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Room name cannot be empty");
                _roomName = value;
            }
        }

        public double SquareFootage
        {
            get => _squareFootage;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Square footage must be positive");
                _squareFootage = value;
            }
        }
        
        public StorageRoomEntity(WarehouseEntity warehouse, string roomName, double squareFootage)
        {
            if (warehouse == null) 
                throw new ArgumentNullException(nameof(warehouse), "Storage Room must belong to a Warehouse.");
            
            _warehouse = warehouse;
            RoomName = roomName;
            SquareFootage = squareFootage;

            
            _warehouse.AddStorageRoom(this); 
        }
        
        
        public void RemoveFromWarehouse()
        {
             _warehouse.RemoveStorageRoom(this);
        }
    }
}