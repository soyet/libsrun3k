using libsrun3k;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PswdEncryptUnitTest
{
	[TestClass]
	public class PswdUnitTest
	{
		[TestMethod]
		public void PswdEncryptTest()
		{
			CampusAuth test = new CampusAuth();

			test.Password = "888888";
			Assert.IsTrue(test.GetEncryptedPswd() == ">cc76ccEDccC");
		}
	}
}
