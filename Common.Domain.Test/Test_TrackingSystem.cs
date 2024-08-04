using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain;

namespace Common.Domain.Test
{
    [TestClass]
    public class Test_TrackingSystem
    {
        private readonly ChangeTracker _changeTracker = new ChangeTracker(); // it must have singleton life time

        [TestMethod]
        public void Test_Tracking()
        {
            foreach (var item in DummyData.orders)
                _changeTracker.Track<Order>(item, EntityTrackState.UnChanged);

            var firstOrder = DummyData.orders.FirstOrDefault(c => c.OrderName!.Contains("first"));

            firstOrder!.AddItem(483838, 48);

            _changeTracker.Track(firstOrder, EntityTrackState.Modified);

            var changedValue = _changeTracker.Getchanges().FirstOrDefault();
        }
    }


    internal static class DummyData
    {
        public static ICollection<Order> orders = new List<Order>
        {
            new Order("first Order ",new EntityCollection<OrderItem>{new OrderItem(10,3),new OrderItem(13,2) }),
            new Order("second Order ",new EntityCollection<OrderItem>{new OrderItem(1560,3),new OrderItem(133,2),new OrderItem(184,2) }),
            new Order("thrid Order ",new EntityCollection<OrderItem>{new OrderItem(5510,3),new OrderItem(413,2) })
        };
    }



    internal sealed class Order : AggregateRoot<int>
    {

        public Order(string? orderName)
            : base(true)
        {
            OrderName = orderName;
            Items.CollectionChanged += NotifyOnOrderCollectionChanged;
        }

        public Order(string? orderName, EntityCollection<OrderItem> items)
            : base(true)
        {
            OrderName = orderName;
            Items = items;
            Items.CollectionChanged += NotifyOnOrderCollectionChanged;
        }

        public string? OrderName
        {
            get => GetValue<string?>();
            private set => SetValue(value);
        }

        public EntityCollection<OrderItem> Items
        {
            get => GetValue<EntityCollection<OrderItem>>();
            private set => SetValue(value);
        }

        public override void AccpetChanges()
        {
            foreach (var item in Items.CurrentList)
                item.AccpetChanges();
            _isChanged = false;
        }

        public void AddItem(int productId, int quantity)
        {
            Items.Add(new OrderItem(productId, quantity));
        }


        private void NotifyOnOrderCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // UpdateChildChanges
        }
    }

    internal sealed class OrderItem : BaseEntity<int>
    {

        public OrderItem(int productId, int quantity)
            : base(true)
        {
            ProductId = productId;
            Quantity = quantity;
        }
        public int ProductId
        {
            get => GetValue<int>();
            private set => SetValue(value);
        }

        public int Quantity
        {
            get => GetValue<int>();
            private set => SetValue(value);
        }

        public override void AccpetChanges()
        {
            _isChanged = false;
        }
    }
}
