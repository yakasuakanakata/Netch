using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using Netch.Controllers;
using Netch.Models;
using Netch.Servers.Trojan.Form;
using Newtonsoft.Json.Linq;

namespace Netch.Servers.Trojan
{
    public class TrojanUtil : IServerUtil
    {
        public ushort Priority { get; } = 2;
        public string TypeName { get; } = "Trojan";
        public string FullName { get; } = "Trojan";
        public string ShortName { get; } = "TR";
        public string[] UriScheme { get; } = {"trojan"};

        public Server ParseJObject(JObject j)
        {
            return j.ToObject<Trojan>();
        }

        public void Edit(Server s)
        {
            new TrojanForm((Trojan) s).ShowDialog();
        }

        public void Create()
        {
            new TrojanForm().ShowDialog();
        }

        public string GetShareLink(Server server)
        {
            // TODO
            return "";
        }

        public IServerController GetController()
        {
            return new TrojanController();
        }

        public IEnumerable<Server> ParseUri(string text)
        {
            var data = new Trojan();

            text = text.Replace("/?", "?");
            try
            {
                if (text.Contains("#"))
                {
                    data.Remark = HttpUtility.UrlDecode(text.Split('#')[1]);
                    text = text.Split('#')[0];
                }

                if (text.Contains("?"))
                {
                    var reg = new Regex(@"^(?<data>.+?)\?(.+)$");
                    var regmatch = reg.Match(text);

                    if (regmatch.Success)
                    {
                        var peer = HttpUtility.UrlDecode(HttpUtility.ParseQueryString(new Uri(text).Query).Get("peer"));

                        if (peer != null)
                            data.Host = peer;

                        text = regmatch.Groups["data"].Value;
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }

                var finder = new Regex(@"^trojan://(?<psk>.+?)@(?<server>.+):(?<port>\d+)");
                var match = finder.Match(text);
                if (!match.Success)
                {
                    throw new FormatException();
                }

                data.Password = match.Groups["psk"].Value;
                data.Hostname = match.Groups["server"].Value;
                data.Port = int.Parse(match.Groups["port"].Value);

                return new[] {data};
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public bool CheckServer(Server s)
        {
            return true;
        }
    }
}