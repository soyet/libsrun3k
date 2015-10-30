using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace libsrun3k
{
	public class CampusAuth
	{
		private string pkey;
		/* private Thread udp_thread; */
		// action
		private string username;
		private string raw_pswd;
		private int drop;
		private int pop;
		private int type;
		private int n;
		private int mbytes;
		private int minutes;
		private int ac_id;
		private string mac;

		// constructor
		public CampusAuth()
		{
			Pkey = "1234567890";

			Username = "";
			Password = "";
			Drop = 0;
			Pop = 0;
			Type = 2;
			N = 117;
			Mbytes = 0;
			Minutes = 0;
			Ac_id = 7;
			MAC = "";
		}

		// setter
		public string Pkey
		{
			get { return pkey; }
			set { pkey = value; }
		}
		public string Username { set { username = value; } }
		public string Password { set { raw_pswd = value; } }
		public int Drop { set { drop = value; } }
		public int Pop { set { pop = value; } }
		public int Type { set { type = value; } }
		public int N { set { n = value; } }
		public int Mbytes { set { mbytes = value; } }
		public int Minutes { set { minutes = value; } }
		public int Ac_id { set { ac_id = value; } }
		public string MAC { set { mac = value; } }

		//
		private string GetLoginParam()
		{
			string lip = "action=login&" +
				"username=" + HttpUtility.UrlEncode(username) + "&" +
				"password=" + HttpUtility.UrlEncode(GetEncryptedPswd()) + "&" +
				"drop=" + HttpUtility.UrlEncode(drop.ToString()) + "&" +
				"pop=" + HttpUtility.UrlEncode(pop.ToString()) + "&" +
				"type=" + HttpUtility.UrlEncode(type.ToString()) + "&" +
				"n=" + HttpUtility.UrlEncode(n.ToString()) + "&" +
				"mbytes=" + HttpUtility.UrlEncode(mbytes.ToString()) + "&" +
				"minutes=" + HttpUtility.UrlEncode(minutes.ToString()) + "&" +
				"ac_id=" + HttpUtility.UrlEncode(ac_id.ToString()) + "&" +
				"mac=" + HttpUtility.UrlEncode(mac);
			return lip;
		}

		private string GetLogoutParam()
		{
			string lop = "action=logout&" +
				"ac_id=" + HttpUtility.UrlEncode(ac_id.ToString()) + "&" +
				"username=" + HttpUtility.UrlEncode(username) + "&" +
				"mac=" + HttpUtility.UrlEncode(mac) + "&" +
				"type=" + HttpUtility.UrlEncode(type.ToString());
			return HttpUtility.UrlEncode(lop);
		}

		public string GetEncryptedPswd()
		{
			string pe = "";
			for (int i = 0; i < raw_pswd.Length; ++i)
			{
				int jn = raw_pswd[i] ^ Pkey[Pkey.Length - i % Pkey.Length - 1];
				char _l = (char)((jn & 0x0f) + 0x36);
				char _h = (char)((jn >> 4 & 0x0f) + 0x63);
				if (i % 2 == 0)
					pe += _l.ToString() + _h.ToString();
				else
					pe += _h.ToString() + _l.ToString();
			}
			return pe;
		}

		public string Login()
		{
			Uri li_uri = new UriBuilder("http", "172.16.154.130", 80, "cgi-bin/srun_portal").Uri;
			return http_action.post(li_uri, GetLoginParam());
		}

		public string Logout()
		{
			Uri lo_uri = new UriBuilder("http", "172.16.154.130", 80, "cgi-bin/srun_portal").Uri;
			return http_action.post(lo_uri, GetLogoutParam());
		}

		public static string GetBroadcastMessage()
		{
			string bmp = HttpUtility.UrlEncode("user_login_name=201416010000");
			Uri bm_uri = new UriBuilder("http", "172.16.154.130", 80, "get_msg.php").Uri;

			return HttpUtility.HtmlDecode(http_action.post(bm_uri, bmp, "gbk"));
		}

		public static string ReadUserInfo()
		{
			Uri ui_uri = new UriBuilder("http", "172.16.154.130", 80, "cgi-bin/rad_user_info").Uri;
			return http_action.post(ui_uri, "");
		}
	}
}

class http_action
{
	public static string get(Uri url, string encoding = "utf-8")
	{
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		request.Method = "GET";
		request.ContentType = "application/x-www-form-urlencoded";

		/* TODO */

		return "";
	}

	public static string post(Uri url, string po_param, string encoding = "utf-8")
	{
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		request.Method = "POST";
		request.ContentType = "application/x-www-form-urlencoded";
		request.ContentLength = Encoding.UTF8.GetByteCount(po_param);

		Stream request_stream = request.GetRequestStream();
		StreamWriter request_sw = new StreamWriter(request_stream);
		request_sw.Write(po_param);
		request_sw.Close();
		request_stream.Close();

		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		Stream response_stream = response.GetResponseStream();
		StreamReader response_sr = new StreamReader(response_stream, Encoding.GetEncoding(encoding));

		string ret_str = response_sr.ReadToEnd();
		response_sr.Close();
		response_stream.Close();

		return ret_str;
	}
}