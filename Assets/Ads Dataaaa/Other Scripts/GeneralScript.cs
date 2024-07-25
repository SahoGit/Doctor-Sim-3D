using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.IO;

public class GeneralScript : MonoBehaviour {

	public static GeneralScript _instance;
    //NativeShare nativeShare;
    Texture2D textureToSave;
    Vector2 previewImageSize;
    DateTime currentDateTime;
    string fileNameByDate;
    string fileName;
    string imageFinalPath;

    public void Awake()
	{
		try{
			if (_instance != null && _instance != this)
			{
                Destroy(this.gameObject);
            } else {
				_instance = this;
			}
		}
		catch(Exception ex) {
			Debug.Log ("Exception Occour : "+ex.Message.ToString());
		}

	}

	public void Start()
	{
        //nativeShare = new NativeShare();
		isExceptionHandlingSetup = false;
		SetupExceptionHandling ();
	}
    private void OnDisable()
    {
        Application.logMessageReceived -= HandleException;
    }

    #region TekeScreenShot
    public void TakeScreenShot(GameObject objectToTempHide=null,GameObject objectToShowOnScreenShot=null)
    {
        currentDateTime = DateTime.Now;
        fileNameByDate = currentDateTime.Year.ToString() + currentDateTime.Month.ToString() + currentDateTime.Day.ToString() + "_" + currentDateTime.Hour.ToString() + currentDateTime.Minute.ToString() + currentDateTime.Second.ToString();
        StartCoroutine(ScreenShot(objectToTempHide, objectToShowOnScreenShot));
    }

    IEnumerator ScreenShot(GameObject objectToTempHide, GameObject objectToShowOnScreenShot)
    {
        if (objectToTempHide != null)
        {
            objectToTempHide.SetActive(false);
        }
        if (objectToShowOnScreenShot != null)
        {
            objectToShowOnScreenShot.SetActive(true);
        }
        yield return new WaitForEndOfFrame();
        textureToSave = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        textureToSave.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        yield return new WaitForEndOfFrame();
        textureToSave.Apply();
        if (objectToTempHide != null)
        {
            objectToTempHide.SetActive(true);
        }
        if (objectToShowOnScreenShot != null)
        {
            objectToShowOnScreenShot.SetActive(false);
        }

        SaveImageForSharing(textureToSave, "GameDistrict", "TempImages" + fileNameByDate);
    }

    void SaveImageForSharing(Texture2D texture, string directoryName, string pictureName)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/" + directoryName))
            Directory.CreateDirectory(Application.persistentDataPath + "/" + directoryName);
        //Debug.Log (Application.persistentDataPath + "/test");
        if (File.Exists(Application.persistentDataPath + "/" + directoryName + "/" + pictureName))
        {
            File.Delete(Application.persistentDataPath + "/" + directoryName + "/" + pictureName);
            //                Debug.Log("Share: File je obrisan");
        }

        byte[] bytes = texture.EncodeToPNG();

        File.WriteAllBytes(Application.persistentDataPath + "/" + directoryName + "/" + pictureName + ".png", bytes);
        imageFinalPath = Application.persistentDataPath + "/" + directoryName + "/" + pictureName + ".png";
        ShareImage();
    }

    void ShareImage()
    {
        //nativeShare.SetText(AssignAdIds_CB.instance.textToShare);
        //nativeShare.AddFile(imageFinalPath);
        //nativeShare.Share();
    }

    #endregion

    #region Exception Handeling
    bool isExceptionHandlingSetup;
	public void SetupExceptionHandling()
	{
		if (!isExceptionHandlingSetup) {
			isExceptionHandlingSetup = true;
            Application.logMessageReceived+=HandleException;
        }
	}

	void HandleException(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Exception)
		{
			//SendEmail(condition + "\n" + stackTrace);
		}
	}
    #endregion

    #region Send Email
    public void SendExceptionEmail(string exception, string function = "function not set")
    {
       // SendEmail(function + "\n" + exception);
    }

    public void SendEmail (string Exception)
	{

		//string device =
		//	SystemInfo.deviceModel + ": deviceModel\n" +
		//	SystemInfo.deviceName + " : deviceName\n" +
		//	SystemInfo.deviceType + ": deviceType\n" +
		//SystemInfo.deviceUniqueIdentifier + "\n" +

		//	SystemInfo.operatingSystem + ": operatingSystem\n" +
		//	SystemInfo.systemMemorySize + " : systemMemorySize\n" +
		//	SystemInfo.processorCount + " : processorCount\n" +
		//	SystemInfo.processorType + ": processorType\n" +

		//	Screen.currentResolution.width + " : currentResolution.width\n" +
		//	Screen.currentResolution.height + " : currentResolution.height\n" +
		//Screen.dpi + "\n" +
		//Screen.fullScreen + "\n" +
		//	SystemInfo.graphicsDeviceName + " : graphicsDeviceName\n" +
		//	SystemInfo.graphicsDeviceVendor + " : graphicsDeviceVendor\n" +
		//	SystemInfo.graphicsMemorySize + " : graphicsDeviceVendor\n" +
		//SystemInfo.maxTextureSize;

		//StartCoroutine (Sending(Exception+"\n"+device));
	}

	//IEnumerator Sending(string errorMessage)
	//{

		//MailMessage mail = new MailMessage();



		//mail.From = new MailAddress("happymelongamesfabric@gmail.com");
		//mail.To.Add("happymelongamesfabric@gmail.com");
		//mail.Subject = "Mom And Baby Washing Laundry Cloth";
		//mail.Body = errorMessage;

		//SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
		//smtpServer.Port = 587;
		//smtpServer.Credentials = new System.Net.NetworkCredential("happymelongamesfabric@gmail.com", "happyMelongames@") as ICredentialsByHost;
		//smtpServer.EnableSsl = true;
		//ServicePointManager.ServerCertificateValidationCallback = 
		//	delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
		//{ return true; };
		//smtpServer.Send(mail);
		//yield return null;
	//}
    #endregion
}
