namespace inventory.Engines.LdapAuth
{
    public class LdapUser
    {
        public string FullName { get; set; }
        public object Username { get; set; }
        public string[] Roles { get; set; }
    }
}