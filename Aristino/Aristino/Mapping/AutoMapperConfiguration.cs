using Aristino.Models;
using Aristino.ViewModel;
using AutoMapper;

namespace Aristino.Mapping
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<ProductVM, Product>().ReverseMap();
            CreateMap<CategoryVM, Category>().ReverseMap();
            CreateMap<Task<List<ProductVM>>, Task<List<Product>>>().ReverseMap();
            CreateMap<Task<List<DiscountBannerVM>>, Task<List<DiscountBanner>>>().ReverseMap();
            CreateMap<DiscountBannerVM, DiscountBanner>().ReverseMap();

            //AccountMap
            CreateMap<Task<List<AccountVM>>, Task<List<Account>>>().ReverseMap();
            CreateMap<AccountVM, Account>().ReverseMap();

            //Roles Map
            CreateMap<Task<List<UserRoleVM>>, Task<List<UserRole>>>().ReverseMap();
            CreateMap<UserRoleVM, UserRole>().ReverseMap();

            //Account - Customer Map
            CreateMap<AccountVM, Customer>().ReverseMap();

            //Customer Map
            CreateMap<CustomerVM, Customer>().ReverseMap();
            CreateMap<Task<List<CustomerVM>>, Task<List<Customer>>>().ReverseMap();
            //Cart Map
            CreateMap<CartVM,Cart>().ReverseMap();
            CreateMap<Task<List<CartVM>>, Task<List<Cart>>>().ReverseMap();

            //Wishtlist Map
            CreateMap<Task<List<WishlistVM>>, Task<List<Wishlist>>>().ReverseMap();
            CreateMap<WishlistVM, Wishlist>().ReverseMap();

            //Blog Map
            CreateMap<Task<List<BlogVM>>, Task<List<Blog>>>().ReverseMap();
            CreateMap<BlogVM, Blog>().ReverseMap();

            //Order Map
            CreateMap<Task<List<OrderVM>>, Task<List<Order>>>().ReverseMap();
            CreateMap<OrderVM, Order>().ReverseMap();


            //Order Detail Map
            CreateMap<Task<List<OrderDetailVM>>, Task<List<OrdersDetail>>>().ReverseMap();
            CreateMap<OrderDetailVM, OrdersDetail>().ReverseMap();

            //Payment Map
            CreateMap<Task<List<PaymentVM>>, Task<List<Payment>>>().ReverseMap();
            CreateMap<PaymentVM,Payment>().ReverseMap();

            //Transaction Map
            CreateMap<Task<List<TransactionStatusVM>>, Task<List<TransactionStatus>>>().ReverseMap();
            CreateMap<TransactionStatusVM, TransactionStatus>().ReverseMap();

            //Comment Map
            CreateMap<Task<List<CommentVM>>,Task<List<Comment>>>().ReverseMap();
            CreateMap<CommentVM, Comment>().ReverseMap();

            //UserStatus Map
            CreateMap<Task<List<UserStatusVM>>, Task<List<UserStatus>>>().ReverseMap();
            CreateMap<UserStatusVM,UserStatus>().ReverseMap();

            //Gender Map
            CreateMap<Task<List<GenderVM>>, Task<List<Gender>>>().ReverseMap();
			CreateMap<GenderVM, Gender>().ReverseMap();

            //Blog Comment Map
            CreateMap<Task<List<BlogCommentVM>>,Task<List<BlogCommentVM>>>().ReverseMap();
            CreateMap<BlogCommentVM,BlogComment>().ReverseMap();

            //Most Sold Product Map
            CreateMap<Task<List<MostSoldProductVM>>, Task<List<MostSoldProduct>>>().ReverseMap();
            CreateMap<MostSoldProductVM, MostSoldProduct>().ReverseMap();

            //FashionCollection Map
            CreateMap<Task<List<FashionCollectionVM>>, Task<List<FashionCollection>>>().ReverseMap();
            CreateMap<FashionCollectionVM,FashionCollection>().ReverseMap();

            //AristinoPolicy Map
            CreateMap<Task<List<AristinoPolicyVM>>, Task<List<AristinoPolicy>>>().ReverseMap();
            CreateMap<AristinoPolicyVM,AristinoPolicy>().ReverseMap();

            //AboutUs Map
            CreateMap<Task<List<AboutUsVM>>, Task<List<AboutU>>>().ReverseMap();
            CreateMap<AboutUsVM, AboutU>().ReverseMap();

            //Color Map
            CreateMap<ColorVM, Color>().ReverseMap();

            //Size Map
            CreateMap<SizeVM, Size>().ReverseMap();
        }
    }
}
