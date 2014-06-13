namespace Profile.Repository.ModelInterfaces
{
   public interface IRegistrationModel
    {
       string Id { get; set; }
       string Email { get; set; }
       string NickName { get; set; }
       string Avatar { get; set; }
       string Password { get; set; }
       string Old_Password { get; set; }
       string Description { get; set; }
       string Culture { get; set; }
    }
}
