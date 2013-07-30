using System;
using System.Collections.Generic;

namespace Signer {
	internal class Program {

		private const string _KEYFILE = "sample-key";

		public static void Main(string[] args) {
			LicenseGenerator licenseGenerator = new LicenseGenerator(_KEYFILE);

			// Create a license that expires in 1 month for a user
			Dictionary<object, object> license = new Dictionary<object, object>();
			license.Add("name", "alex");
			license.Add("expires", DateTime.Now.AddMonths(1).ToString());
			licenseGenerator.Sign(license);

			// Write signed license to console. Give out to user as XML/JSON/Base64 file
			foreach (var member in license) {
				Console.WriteLine("{0}: {1}", member.Key, member.Value);
			}

			// Show that the license has not been touched in the meantime
			Console.WriteLine(licenseGenerator.Verify(license));
		}
	}
}
