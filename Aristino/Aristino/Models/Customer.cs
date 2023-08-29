using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class Customer
{
    public int CustomersId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? Dob { get; set; }

    public string? CustomerAddress { get; set; }

    public int? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? Avatar { get; set; }

    public int? GenderId { get; set; }

    public int? StatusId { get; set; }

    public string? CardNumber { get; set; }

    public string? CardOwner { get; set; }

    public string? ExpiredDate { get; set; }

    public int? Cvc { get; set; }

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();

    public virtual ICollection<BlogComment> BlogComments { get; } = new List<BlogComment>();

    public virtual ICollection<Blog> Blogs { get; } = new List<Blog>();

    public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual Gender? Gender { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual UserStatus? Status { get; set; }

    public virtual ICollection<Wishlist> Wishlists { get; } = new List<Wishlist>();
}
