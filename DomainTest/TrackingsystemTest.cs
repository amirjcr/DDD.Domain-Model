using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainTest
{
    [TestClass]
    public class TrackingsystemTest
    {
        private Book Book = new Book(new EntityCollection<BookImage>
        {
            new BookImage("ImageOne"),
            new BookImage("ImageTwo")
        });


        [TestMethod]
        public void Test_Tracking_Child()
        {
            var childChanges = Book.Images!.Changes;

            Book.AddImage(new BookImage("ImageThree"));

        }
    }

    internal class Book : AggregateRoot<long>
    {
        public Book(EntityCollection<BookImage> images)
            : base(true)
        {
            Images = images;
        }

        public EntityCollection<BookImage>? Images
        {
            get => GetValue<EntityCollection<BookImage>?>();
            private set => SetValue(value);
        }


        public void AddImage(BookImage bookImage) => Images?.Add(bookImage);
    }


    internal class BookImage : BaseEntity<int>
    {
        public BookImage(string imageName)
            : base(true)
        {
            ImageName = imageName;
        }

        public string? ImageName
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }
    }



}
