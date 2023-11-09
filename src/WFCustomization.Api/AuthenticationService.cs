using Novell.Directory.Ldap;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace inventory.Engines.LdapAuth
{
    public class LdapAuthenticationService //: ILdapAuthenticationService
    {

        private const string MemberOfAttribute = "memberOf";
        private const string FirstNameAttribute = "GIVENNAME";
        private const string LastNameAttribute = "SN";
        private const string SAMAccountNameAttribute = "sAMAccountName";

        //private readonly LdapConfig _config;
        private readonly Novell.Directory.Ldap.LdapConnection _connection;

       // private readonly IStringLocalizer<SharedResource> _localizer;
        public LdapAuthenticationService()
        {
            _connection = new LdapConnection();
        }

        public LdapUser Login(string username, string password)
        {
            try
            {
                _connection.Connect("10.30.20.46", LdapConnection.DEFAULT_PORT);
                _connection.Bind("CN=Abobakr Mohammed,CN=Users,DC=MNCCDEV,DC=com", password);

                var result = _connection.Search("",
                    LdapConnection.SCOPE_SUB,
                    "(objectClass=person)",
                    new[] {
                    MemberOfAttribute,
                    //FirstNameAttribute,
                    //LastNameAttribute,
                    //SAMAccountNameAttribute
                    },
                    false
                );

                try
                {
                    var user = result.next();
                    if (user != null)
                    {
                        try
                        {
                            _connection.Bind(user.DN, password);
                        }
                        catch
                        {
                            throw new UnauthorizedAccessException("InvalidUserNameOrPasswordException");
                        }

                        if (_connection.Bound)
                        {

                            var accountNameAttr = user.getAttribute(SAMAccountNameAttribute);
                            if (accountNameAttr == null)
                            {
                                throw new UnauthorizedAccessException("Your account is missing the account name.");
                            }
                            LdapAttribute firstNameAttr = null;
                            LdapAttribute lastNameAttr = null;
                            LdapAttribute memberAttr = null;
                            try
                            {
                                firstNameAttr = user.getAttribute(FirstNameAttribute);
                            }
                            catch { firstNameAttr = null; }
                            var firstNameValue = firstNameAttr != null ? firstNameAttr.StringValue : "";
                            try
                            {
                                lastNameAttr = user.getAttribute(LastNameAttribute);
                            }
                            catch { lastNameAttr = null; }

                            var lastNameValue = lastNameAttr != null ? lastNameAttr.StringValue : "";
                            try
                            {
                                memberAttr = user.getAttribute(MemberOfAttribute);
                            }
                            catch { memberAttr = null; }


                            return new LdapUser
                            {
                                FullName = firstNameValue + " " + lastNameValue,
                                Username = accountNameAttr.StringValue,
                                Roles = memberAttr?.StringValueArray
                                    .Select(x => GetGroup(x))
                                    .Where(x => x != null)
                                    .Distinct()
                                    .ToArray()
                            };
                        }
                    }
                }
                catch
                {
                    throw new UnauthorizedAccessException("InvalidUserNameOrPasswordException");
                }
                finally
                {
                    _connection.Disconnect();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(ex.Message + ex.InnerException?.Message);
            }
        }

        //public string GetUserFullName(string username)
        //{
        //    try
        //    {
        //        _connection.Connect(_config.Url, LdapConnection.DefaultPort);
        //        _connection.Bind(_config.Username, _config.Password);

        //        var searchFilter = String.Format(_config.SearchFilter, username);
        //        var result = _connection.Search(
        //            _config.SearchBase,
        //            LdapConnection.ScopeSub,
        //            searchFilter,
        //            new[] {
        //            FirstNameAttribute,
        //            LastNameAttribute
        //            },
        //            false
        //        );

        //        try
        //        {
        //            var user = result.Next();
        //            if (user != null)
        //            {
        //                if (_connection.Bound)
        //                {
        //                    LdapAttribute firstNameAttr = null;
        //                    LdapAttribute lastNameAttr = null;
        //                    try
        //                    {
        //                        firstNameAttr = user.GetAttribute(FirstNameAttribute);
        //                    }
        //                    catch { firstNameAttr = null; }
        //                    var firstNameValue = firstNameAttr != null ? firstNameAttr.StringValue : "";
        //                    try
        //                    {
        //                        lastNameAttr = user.GetAttribute(LastNameAttribute);
        //                    }
        //                    catch { lastNameAttr = null; }

        //                    var lastNameValue = lastNameAttr != null ? lastNameAttr.StringValue : "";



        //                    return firstNameValue + " " + lastNameValue;
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            throw new InvalidUserNameOrPasswordException(_localizer["InvalidUserNameOrPasswordException"]);
        //        }
        //        finally
        //        {
        //            _connection.Disconnect();
        //        }

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidUserNameOrPasswordException(ex.Message + ex.InnerException?.Message);
        //    }
        //}
        private string GetGroup(string value)
        {
            Match match = Regex.Match(value, "^CN=([^,]*)");
            if (!match.Success)
            {
                return null;
            }

            return match.Groups[1].Value;
        }

        //public List<string> GetUserGroups(string userName)
        //{
        //    var groups = new List<string>();

        //    _connection.Connect(_config.Url, LdapConnection.DefaultPort);
        //    _connection.Bind(_config.Username, _config.Password);
        //    LdapSearchConstraints cons = _connection.SearchConstraints;
        //    cons.ReferralFollowing = true;
        //    _connection.Constraints = cons;

        //    var searchFilter = String.Format(_config.SearchFilter, userName);
        //    var result = _connection.Search(
        //        _config.SearchBase,
        //        LdapConnection.ScopeSub,
        //        searchFilter,
        //        new[] {
        //            MemberOfAttribute,
        //            FirstNameAttribute,
        //            LastNameAttribute,
        //            SAMAccountNameAttribute

        //        },
        //        false
        //    );

        //    try
        //    {
        //        var user = result.Next();
        //        if (user != null)
        //        {

        //            var memberAttr = user.GetAttribute(MemberOfAttribute);
        //            if (memberAttr == null)
        //            {
        //                throw new UnauthorizedAccessException("Your account is missing roles.");
        //            }

        //            groups = memberAttr.StringValueArray
        //                    .Select(x => GetGroup(x))
        //                    .Where(x => x != null)
        //                    .Distinct()
        //                    .ToList();
        //        }
        //    }
        //    finally
        //    {
        //        _connection.Disconnect();
        //    }

        //    return groups;
        //}

        //public List<LdapUser> GetUserNames(string userSearchToken)
        //{

        //    var lst = new List<LdapUser>() { };
        //    try
        //    {
        //        using (LdapConnection obj = new LdapConnection())
        //        {
        //            obj.Connect(_config.Url, LdapConnection.DefaultPort);
        //            obj.Bind(_config.Username, _config.Password);

        //            var searchTerm = "(&(objectClass=user)(SAMACCOUNTNAME=*" + userSearchToken + "*))";
        //            var lsc = obj.Search(_config.BindDn, LdapConnection.ScopeSub, searchTerm, new[] {
        //            MemberOfAttribute,
        //            FirstNameAttribute,
        //            LastNameAttribute,
        //            SAMAccountNameAttribute

        //        }, false);

        //            //System.Threading.Thread.Sleep(1000);

        //            while (lsc.HasMore())
        //            {
        //                var user = lsc.Next();
        //                var accountNameAttr = user.GetAttribute(SAMAccountNameAttribute);
        //                if (accountNameAttr == null)
        //                {
        //                    throw new UnauthorizedAccessException("Your account is missing the account name.");
        //                }
        //                LdapAttribute firstNameAttr = null;
        //                LdapAttribute lastNameAttr = null;
        //                LdapAttribute memberAttr = null;
        //                try
        //                {
        //                    firstNameAttr = user.GetAttribute(FirstNameAttribute);
        //                }
        //                catch { firstNameAttr = null; }

        //                var firstNameValue = firstNameAttr != null ? firstNameAttr.StringValue : "";
        //                try
        //                {
        //                    lastNameAttr = user.GetAttribute(LastNameAttribute);
        //                }
        //                catch { lastNameAttr = null; }
        //                var lastNameValue = lastNameAttr != null ? lastNameAttr.StringValue : "";
        //                try
        //                {
        //                    memberAttr = user.GetAttribute(MemberOfAttribute);
        //                }
        //                catch { memberAttr = null; }

        //                var ldapUser = new LdapUser
        //                {
        //                    FullName = firstNameValue + " " + lastNameValue,
        //                    Username = accountNameAttr.StringValue,
        //                    Roles = memberAttr?.StringValueArray
        //                        .Select(x => GetGroup(x))
        //                        .Where(x => x != null)
        //                        .Distinct()
        //                        .ToArray()
        //                };
        //                lst.Add(ldapUser);//.getAttribute("cn").StringValue);

        //            }
        //            return lst;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return
        //        new List<LdapUser> { };
        //}
        //public List<LdapUser> GetTechnicanUserNames(string userSearchToken)
        //{

        //    var lst = new List<LdapUser>() { };
        //    try
        //    {
        //        using (LdapConnection obj = new LdapConnection())
        //        {
        //            obj.Connect(_config.Url, LdapConnection.DefaultPort);
        //            obj.Bind(_config.Username, _config.Password);

        //            var searchTerm = string.IsNullOrEmpty(userSearchToken) ?
        //                "(&(objectClass=user)(SAMACCOUNTNAME=*))" :
        //                "(&(objectClass=user)(SAMACCOUNTNAME=*" + userSearchToken + "*))";

        //            var lsc = obj.Search(_config.BindDn, LdapConnection.ScopeSub, searchTerm, new[] {
        //            MemberOfAttribute,
        //            FirstNameAttribute,
        //            LastNameAttribute,
        //            SAMAccountNameAttribute

        //        }, false);

        //            //System.Threading.Thread.Sleep(1000);

        //            while (lsc.HasMore())
        //            {
        //                var user = lsc.Next();
        //                var accountNameAttr = user.GetAttribute(SAMAccountNameAttribute);
        //                if (accountNameAttr == null)
        //                {
        //                    throw new UnauthorizedAccessException("Your account is missing the account name.");
        //                }
        //                LdapAttribute firstNameAttr = null;
        //                LdapAttribute lastNameAttr = null;
        //                LdapAttribute memberAttr = null;
        //                try
        //                {
        //                    firstNameAttr = user.GetAttribute(FirstNameAttribute);
        //                }
        //                catch { firstNameAttr = null; }

        //                var firstNameValue = firstNameAttr != null ? firstNameAttr.StringValue : "";
        //                try
        //                {
        //                    lastNameAttr = user.GetAttribute(LastNameAttribute);
        //                }
        //                catch { lastNameAttr = null; }
        //                var lastNameValue = lastNameAttr != null ? lastNameAttr.StringValue : "";
        //                try
        //                {
        //                    memberAttr = user.GetAttribute(MemberOfAttribute);
        //                }
        //                catch { memberAttr = null; }

        //                var userRoles = memberAttr?.StringValueArray
        //                        .Select(x => GetGroup(x))
        //                        .Where(x => x == UserGroups.TechnicalDepartments)
        //                        .Distinct()
        //                        .ToArray();

        //                if (userRoles == null || userRoles.Count() == 0)
        //                    continue;

        //                var ldapUser = new LdapUser
        //                {
        //                    FullName = firstNameValue + " " + lastNameValue,
        //                    Username = accountNameAttr.StringValue,
        //                    Roles = userRoles
        //                };
        //                lst.Add(ldapUser);//.getAttribute("cn").StringValue);

        //            }
        //            return lst;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return
        //        new List<LdapUser> { };
        //}
        //public List<LdapUser> GetAssistantTechnicanUserNames(string userSearchToken)
        //{

        //    var lst = new List<LdapUser>() { };
        //    try
        //    {
        //        using (LdapConnection obj = new LdapConnection())
        //        {
        //            obj.Connect(_config.Url, LdapConnection.DefaultPort);
        //            obj.Bind(_config.Username, _config.Password);

        //            var searchTerm = "(&(objectClass=user)(SAMACCOUNTNAME=*" + userSearchToken + "*))";
        //            var lsc = obj.Search(_config.BindDn, LdapConnection.ScopeSub, searchTerm, new[] {
        //            MemberOfAttribute,
        //            FirstNameAttribute,
        //            LastNameAttribute,
        //            SAMAccountNameAttribute

        //        }, false);

        //            //System.Threading.Thread.Sleep(1000);

        //            while (lsc.HasMore())
        //            {
        //                var user = lsc.Next();
        //                var accountNameAttr = user.GetAttribute(SAMAccountNameAttribute);
        //                if (accountNameAttr == null)
        //                {
        //                    throw new UnauthorizedAccessException("Your account is missing the account name.");
        //                }
        //                LdapAttribute firstNameAttr = null;
        //                LdapAttribute lastNameAttr = null;
        //                LdapAttribute memberAttr = null;
        //                try
        //                {
        //                    firstNameAttr = user.GetAttribute(FirstNameAttribute);
        //                }
        //                catch { firstNameAttr = null; }

        //                var firstNameValue = firstNameAttr != null ? firstNameAttr.StringValue : "";
        //                try
        //                {
        //                    lastNameAttr = user.GetAttribute(LastNameAttribute);
        //                }
        //                catch { lastNameAttr = null; }
        //                var lastNameValue = lastNameAttr != null ? lastNameAttr.StringValue : "";
        //                try
        //                {
        //                    memberAttr = user.GetAttribute(MemberOfAttribute);
        //                }
        //                catch { memberAttr = null; }

        //                var userRoles = memberAttr?.StringValueArray
        //                        .Select(x => GetGroup(x))
        //                        .Where(x => x == UserGroups.AssistantTechnicalDepartments)
        //                        .Distinct()
        //                        .ToArray();

        //                if (userRoles == null || userRoles.Count() == 0)
        //                    continue;

        //                var ldapUser = new LdapUser
        //                {
        //                    FullName = firstNameValue + " " + lastNameValue,
        //                    Username = accountNameAttr.StringValue,
        //                    Roles = userRoles
        //                };
        //                lst.Add(ldapUser);//.getAttribute("cn").StringValue);

        //            }
        //            return lst;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return
        //        new List<LdapUser> { };
        //}

        //public List<LdapUser> GetStoreKeeperUserNames(string userSearchToken)
        //{

        //    var lst = new List<LdapUser>() { };
        //    try
        //    {
        //        using (LdapConnection obj = new LdapConnection())
        //        {
        //            obj.Connect(_config.Url, LdapConnection.DEFAULT_PORT);
        //            obj.Bind(_config.Username, _config.Password);

        //            var searchTerm = "(&(objectClass=user)(SAMACCOUNTNAME=*" + userSearchToken + "*))";
        //            var lsc = obj.Search(_config.BindDn, LdapConnection.SCOPE_SUB, searchTerm, new[] {
        //            MemberOfAttribute,
        //            FirstNameAttribute,
        //            LastNameAttribute,
        //            SAMAccountNameAttribute

        //        }, false);

        //            //System.Threading.Thread.Sleep(1000);

        //            while (lsc.HasMore())
        //            {
        //                var user = lsc.Next();
        //                var accountNameAttr = user.GetAttribute(SAMAccountNameAttribute);
        //                if (accountNameAttr == null)
        //                {
        //                    throw new UnauthorizedAccessException("Your account is missing the account name.");
        //                }
        //                LdapAttribute firstNameAttr = null;
        //                LdapAttribute lastNameAttr = null;
        //                LdapAttribute memberAttr = null;
        //                try
        //                {
        //                    firstNameAttr = user.GetAttribute(FirstNameAttribute);
        //                }
        //                catch { firstNameAttr = null; }

        //                var firstNameValue = firstNameAttr != null ? firstNameAttr.StringValue : "";
        //                try
        //                {
        //                    lastNameAttr = user.GetAttribute(LastNameAttribute);
        //                }
        //                catch { lastNameAttr = null; }
        //                var lastNameValue = lastNameAttr != null ? lastNameAttr.StringValue : "";
        //                try
        //                {
        //                    memberAttr = user.GetAttribute(MemberOfAttribute);
        //                }
        //                catch { memberAttr = null; }

        //                var userRoles = memberAttr?.StringValueArray
        //                        .Select(x => GetGroup(x))
        //                        .Where(x => x == UserGroups.StoreKeeper)
        //                        .Distinct()
        //                        .ToArray();

        //                if (userRoles == null || userRoles.Count() == 0)
        //                    continue;

        //                var ldapUser = new LdapUser
        //                {
        //                    FullName = firstNameValue + " " + lastNameValue,
        //                    Username = accountNameAttr.StringValue,
        //                    Roles = userRoles
        //                };
        //                lst.Add(ldapUser);//.getAttribute("cn").StringValue);

        //            }
        //            return lst;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return
        //        new List<LdapUser> { };
        //}
        //public List<LdapUser> GetWarehouseManager()
        //{

        //    var lst = new List<LdapUser>() { };
        //    try
        //    {
        //        using (LdapConnection obj = new LdapConnection())
        //        {
        //            obj.Connect(_config.Url, LdapConnection.DefaultPort);
        //            obj.Bind(_config.Username, _config.Password);

        //            var searchTerm = "(&(objectClass=user)(SAMACCOUNTNAME=*))";
        //            var lsc = obj.Search(_config.BindDn, LdapConnection.ScopeSub, searchTerm, new[] {
        //            MemberOfAttribute,
        //            FirstNameAttribute,
        //            LastNameAttribute,
        //            SAMAccountNameAttribute

        //        }, false);

        //            //System.Threading.Thread.Sleep(1000);

        //            while (lsc.HasMore())
        //            {
        //                var user = lsc.Next();
        //                var accountNameAttr = user.GetAttribute(SAMAccountNameAttribute);
        //                if (accountNameAttr == null)
        //                {
        //                    throw new UnauthorizedAccessException("Your account is missing the account name.");
        //                }
        //                LdapAttribute firstNameAttr = null;
        //                LdapAttribute lastNameAttr = null;
        //                LdapAttribute memberAttr = null;
        //                try
        //                {
        //                    firstNameAttr = user.GetAttribute(FirstNameAttribute);
        //                }
        //                catch { firstNameAttr = null; }

        //                var firstNameValue = firstNameAttr != null ? firstNameAttr.StringValue : "";
        //                try
        //                {
        //                    lastNameAttr = user.GetAttribute(LastNameAttribute);
        //                }
        //                catch { lastNameAttr = null; }
        //                var lastNameValue = lastNameAttr != null ? lastNameAttr.StringValue : "";
        //                try
        //                {
        //                    memberAttr = user.GetAttribute(MemberOfAttribute);
        //                }
        //                catch { memberAttr = null; }

        //                var userRoles = memberAttr?.StringValueArray
        //                        .Select(x => GetGroup(x))
        //                        .Where(x => x == UserGroups.WarehouseManager)
        //                        .Distinct()
        //                        .ToArray();

        //                if (userRoles == null || userRoles.Count() == 0)
        //                    continue;

        //                var ldapUser = new LdapUser
        //                {
        //                    FullName = firstNameValue + " " + lastNameValue,
        //                    Username = accountNameAttr.StringValue,
        //                    Roles = userRoles
        //                };
        //                lst.Add(ldapUser);//.getAttribute("cn").StringValue);

        //            }
        //            return lst;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return
        //        new List<LdapUser> { };
        //}
    }
}
