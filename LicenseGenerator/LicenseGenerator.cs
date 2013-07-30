using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace Signer {
	public class LicenseGenerator {
		private readonly RSACryptoServiceProvider _cryptoServiceProvider = new RSACryptoServiceProvider();
		private readonly JavaScriptSerializer _javaScriptSerializer = new JavaScriptSerializer();

		public LicenseGenerator(String keyFile) {
			StreamReader sw = new StreamReader(keyFile);
			//possible null value fix later
			_cryptoServiceProvider.FromXmlString(sw.ReadLine());
		}

		public Dictionary<object, object> Sign(Dictionary<object, object> licenseInfo)
		{
			string licenseAsJson = _javaScriptSerializer.Serialize(licenseInfo);
			string signature = Convert.ToBase64String(_cryptoServiceProvider
								.SignData(Encoding.UTF8.GetBytes(licenseAsJson),
								new SHA512CryptoServiceProvider()));

			licenseInfo.Add("signature", signature);
			return licenseInfo;
		}

		public bool Verify(Dictionary<Object, Object> signedLicense) {
			//possible null fix later
			byte[] signature = Convert.FromBase64String(signedLicense["signature"] as string);
			Dictionary<Object, Object> strippedLicense = new Dictionary<object, object>(signedLicense);
			strippedLicense.Remove("signature");
			byte[] licenseInfo = Encoding.UTF8.GetBytes(_javaScriptSerializer.Serialize(strippedLicense));
			return _cryptoServiceProvider.VerifyData(licenseInfo, new SHA512CryptoServiceProvider(), signature);
		}

		static void CreateKeyPair(String keyFile) {
			// could throw exception if file is not able to be created
			StreamWriter sw = new StreamWriter(keyFile, false);
			sw.WriteLine(new RSACryptoServiceProvider().ToXmlString(true));
			sw.Close();
		}
	}
}
