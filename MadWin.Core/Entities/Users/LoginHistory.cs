namespace MadWin.Core.Entities.Users
{
   public class LoginHistory
    {
        #region ctor
        public LoginHistory()
        {
                
        }
        #endregion
        #region Field
        public int LoginHistoryId { get; set; }

        public int UserId { get; set; }

        public string Url { get; set; }

        public string CookieId { get; set; }
        #endregion
        #region Relationship
        public User User { get; set; }
        #endregion
    }
}
