using System;
using System.Configuration;
using System.Text;
using Profile.Repository.DataProvider;
using Profile.Repository.Entity;
using Profile.Repository.ModelInterfaces;

namespace Profile.Repository
{
   public class SqlProfileRepository
   {
       private string _conn = ConfigurationManager.ConnectionStrings["MySqlWrioProfile"].ConnectionString;

       public UserAccount GetUserBy(string id)
       {
           UserAccount user;
           using (var context = new ProfileDbManager(_conn))
           {
               user = context.SetCommand(
                   string.Format("SELECT * FROM UserAccount WHERE Id = '{0}'", id)).
                   ExecuteObject<UserAccount>();
           }
           return user;
       }
       public UserAccount GetUsersBy(string email)
       {
           UserAccount user;
           using (var context = new ProfileDbManager(_conn))
           {
               try
               {
                   user = context.SetCommand(
                  string.Format("SELECT * FROM UserAccount WHERE Email = '{0}' ", email)).
                  ExecuteObject<UserAccount>();
               }catch(Exception e)
               {
                   throw new Exception(e.Message);
               }
              
           }
           return user;
       }
       public UserAccount GetUserId(string userIdentity)
       {
           return GetUserBy(userIdentity);
       }
       public void UpdateUser(UserAccount user)
       {
           var queryBuilder = new StringBuilder();
           queryBuilder.Append("UPDATE UserAccount SET ");
           queryBuilder.Append("RegistrationDate = @RegistrationDate, ");
           queryBuilder.Append("LoginDate = @LoginDate, ");
           queryBuilder.Append("Email = @Email, ");
           queryBuilder.Append("NickName = @NickName, ");
           queryBuilder.Append("PasswordSalt = @PasswordSalt, ");
           queryBuilder.Append("PasswordHash = @PasswordHash, ");
           queryBuilder.Append("Hash = @Hash, ");
           queryBuilder.Append("GoogleProfile = @GoogleProfile, ");
           queryBuilder.Append("FacebookProfile = @FacebookProfile, ");
           queryBuilder.Append("TwitterProfile = @TwitterProfile, ");
           queryBuilder.Append("SessionId = @SessionId, ");
           queryBuilder.Append("Session = @Session, ");
           queryBuilder.Append("Culture = @Culture, ");
           queryBuilder.Append("Avatar = @Avatar, ");
           queryBuilder.Append("Description = @Description, ");
           queryBuilder.Append("Provider = @Provider ");
           queryBuilder.Append("WHERE Id = @Id");
           using (var db = new ProfileDbManager(_conn))
           {

               db.SetCommand(queryBuilder.ToString(), db.CreateParameters(user)).
                   ExecuteNonQuery();
           }
       }
       public void UpdateUser(DateTime date, string salt, string hash, IRegistrationModel model)
       {
           var entity = new UserAccount();
           entity.Id = model.Id;
           entity.RegistrationDate = date;
           entity.PasswordSalt = salt;
           entity.PasswordHash = hash;
           entity.Avatar = model.Avatar;
           entity.Description = model.Description;
           entity.Email = model.Email;
           entity.NickName = model.NickName;
           var queryBuilder = new StringBuilder();
           queryBuilder.Append("UPDATE UserAccount SET ");
           queryBuilder.Append("RegistrationDate = @RegistrationDate, ");
           queryBuilder.Append("Email = @Email, ");
           queryBuilder.Append("NickName = @NickName, ");
           queryBuilder.Append("PasswordSalt = @PasswordSalt, ");
           queryBuilder.Append("PasswordHash = @PasswordHash, ");
           queryBuilder.Append("Avatar = @Avatar, ");
           queryBuilder.Append("Description = @Description ");
           queryBuilder.Append("WHERE Id = @Id");
           using (var db = new ProfileDbManager(_conn))
           {
               db.SetCommand(queryBuilder.ToString(), db.CreateParameters(entity)).
                   ExecuteNonQuery();
           }
       }
       public void UpdateUser(string salt,string hash,UserAccount curUser)
       {
           var entity = new UserAccount();
           entity.Id = curUser.Id;
           entity.PasswordSalt = salt;
           entity.PasswordHash = hash;
           var queryBuilder = new StringBuilder();
           queryBuilder.Append("UPDATE UserAccount SET ");
           queryBuilder.Append("PasswordSalt = @PasswordSalt, ");
           queryBuilder.Append("PasswordHash = @PasswordHash ");
           queryBuilder.Append("WHERE Id = @Id");
           using (var db = new ProfileDbManager(_conn))
           {
               db.SetCommand(queryBuilder.ToString(), db.CreateParameters(entity)).
                   ExecuteNonQuery();
           }
       }
       public void SaveUser(UserAccount user)
       {
           try
           {
               using (var db = new ProfileDbManager(_conn))
               {
                   db.SetCommand(@"
                    INSERT INTO UserAccount ( 
                    Id, RegistrationDate, LoginDate,Email,NickName,
                    PasswordSalt,PasswordHash,Hash,GoogleProfile,FacebookProfile,TwitterProfile,SessionId,Session,
                    Culture,Avatar,Description,Provider)
                    VALUES (@Id, @RegistrationDate, @LoginDate,@Email,@NickName,
                    @PasswordSalt,@PasswordHash,@Hash,@GoogleProfile,@FacebookProfile,@TwitterProfile,@SessionId,@Session,
                    @Culture,@Avatar,@Description,@Provider)",
                         db.Parameter("@Id", user.Id),
                         db.Parameter("@RegistrationDate", user.RegistrationDate),
                         db.Parameter("@LoginDate", user.LoginDate),
                         db.Parameter("@Email", user.Email),
                         db.Parameter("@NickName", user.NickName),
                         db.Parameter("@PasswordSalt", user.PasswordSalt),
                         db.Parameter("@PasswordHash", user.PasswordHash),
                         db.Parameter("@Hash", user.Hash),
                         db.Parameter("@GoogleProfile", user.GoogleProfile),
                         db.Parameter("@FacebookProfile", user.FacebookProfile),
                         db.Parameter("@TwitterProfile", user.TwitterProfile),
                         db.Parameter("@SessionId", user.SessionId),
                         db.Parameter("@Session", user.Session),
                         db.Parameter("@Avatar", user.Avatar),
                         db.Parameter("@Culture", user.Culture),
                         db.Parameter("@Description", user.Description),
                         db.Parameter("@Provider", user.Provider))
                     .ExecuteNonQuery();
               }
           }
           catch (Exception e)
           {               
               throw;
           }
       }
       public UserAccount SaveUser(DateTime date, string salt, string hash, IRegistrationModel model, string facebookId = null, string googleId = null, string twitterId = null)
       {
           var user = new UserAccount();
           user.Id = model.Id;
           user.NickName = model.NickName;
           user.Culture = model.Culture;
           user.Description = model.Description;
           user.Avatar = model.Avatar;
           user.SessionId = string.Empty;
           user.PasswordSalt = salt;
           user.PasswordHash = hash;
           user.LoginDate = date;
           user.RegistrationDate = date;
           user.Hash = Guid.NewGuid().ToString("N");
           user.Email = model.Email;
           //if (!string.IsNullOrEmpty(facebookId))
           //    user.FacebookProfile = facebookId;
           //user.Provider = OAuthProvider.FaceBook.ToString();
           //if (!string.IsNullOrEmpty(googleId))
           //    user.GoogleProfile = googleId;
           //if (!string.IsNullOrEmpty(twitterId))
           //    user.TwitterProfile = twitterId;
           SaveUser(user);
           return user;
       }
       public void Delete(string id)
       {
           var query = string.Format("DELETE FROM UserAccount WHERE Id= '{0}'", id);
           using (var db = new ProfileDbManager(_conn))
           {
               db.SetCommand(query).ExecuteNonQuery();
           }
       }
       public bool IsEmailExist(string email)
       {
           var user = GetUsersBy(email);
           if(user == null)
           {
               return false;
           }
           return true;
       }
    }
}
