using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

using Google;

public class GoogleSSOHandler : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Text StatusText;

    [SerializeField]
    private string WebClientId = "<your client id here>";

    private string status = "";

    private GoogleSignInConfiguration configuration;
    private string googleIdToken = string.Empty;

    Firebase.Auth.FirebaseAuth auth = null;

    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = WebClientId,
            RequestIdToken = true
        };

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    void Update()
    {
        StatusText.text = status;


    }

    public void SignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        status += "Calling SignIn\n";

        GoogleSignIn.DefaultInstance.SignIn()
        .ContinueWith(SignInFinished);
    }

    private void SignInFinished(Task<GoogleSignInUser> signInTask)
    {
        if (signInTask.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator = signInTask.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    status += "Error: " + error.Status + " " + error.Message + "\n";
                }
                else
                {
                    status += "Got unexpected error " + signInTask.Exception + "\n";
                }
            }
        }
        else if (signInTask.IsCanceled)
        {
            status += "Sign in request was canceled\n";
        }
        else
        {
            status += "Success! Welcome " + signInTask.Result.DisplayName + ".\n";
            status += "Id token: " + signInTask.Result.IdToken;

            SignInFirebase(signInTask.Result.IdToken, null);
        }
    }

    private void SignInFirebase(string googleIdToken, string googleAccessToken)
    {
        status += "Calling SignInFirebase\n";

        Firebase.Auth.Credential credential = null;
        try{
            credential = Firebase.Auth.GoogleAuthProvider.GetCredential(googleIdToken, null);
            status += "Credential ready\n";
        }
        catch(System.Exception e)
        {
            status += e.Message + "\n";
        }
        
        auth.SignInWithCredentialAsync(credential)
        .ContinueWith( task => {
            if (task.IsCanceled)
            {
                status += "SignInWithCredentialAsync was canceled.";
            }
            else if (task.IsFaulted)
            {
                status += "SignInWithCredentialAsync encountered an error: " + task.Exception;
            }
            else
            {
                Firebase.Auth.FirebaseUser newUser = task.Result;
                status += "Registered as firebase user successfully! Welcome " + newUser.DisplayName + " (" + newUser.UserId + ")";
            }
        });

    }
}
