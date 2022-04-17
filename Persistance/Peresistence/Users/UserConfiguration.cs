//using Core.Users;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Persistance.Users
//{
//    public class UserConfiguration : IEntityTypeConfiguration<User>
//    {
//        public void Configure(EntityTypeBuilder<User> builder)
//        {
//            builder.Property(x=>x.UserId).IsRequired();
//            builder.HasOne<IdentityUser>(nameof(User.UserId)).
//                WithOne(nameof(IdentityUser.Id)).HasForeignKey<IdentityUser>();
//        }
//    }
//}
