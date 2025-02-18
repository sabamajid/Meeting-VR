using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class InputUIManager : MonoBehaviour
{
    [Header("Signin Input Area")]
    public TMP_InputField signInEmailInputField;
    public TMP_InputField signInPasswordInputField;

    [Header("SignUp Input Area")]
    public TMP_InputField firstNameInputField;
    public TMP_InputField secondNameInputField;
    public TMP_InputField signUpEmailInputField;
    public TMP_InputField contactInputField;
    public TMP_InputField signUpPasswordInputField;
    public TMP_InputField confirmPasswordInputField;

    private const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";


    public void Next()
    {
        if (string.IsNullOrEmpty(firstNameInputField.text) || string.IsNullOrEmpty(secondNameInputField.text))
        {
            Debug.Log("Name fields cannot be empty.");
            return;
        }
        if (string.IsNullOrEmpty(signUpEmailInputField.text))
        {
            Debug.Log("Email field cannot be empty.");
            return;
        }
        if (!ValidateEmail(signUpEmailInputField.text))
        {
            Debug.Log("Invalid email format.");
            return;
        }
     
    }

    public void SignInUser()
    {
        if (string.IsNullOrEmpty(signInEmailInputField.text))
        {
            Debug.Log("Email field cannot be empty.");
            return;
        }
        if (!ValidateEmail(signInEmailInputField.text))
        {
            Debug.Log("Invalid email format.");
            return;
        }
        if (string.IsNullOrEmpty(signInPasswordInputField.text))
        {
            Debug.Log("Password cannot be empty.");
            return;
        }
        if (signInPasswordInputField.text.Length < 8)
        {
            Debug.Log("Password must be at least 8 characters long.");
            return;
        }
        
  AuthManager.instance.LoginUser(signInEmailInputField.text.ToLower(), signInPasswordInputField.text);
    }

    public void CreateUser()
    {
        if (string.IsNullOrEmpty(firstNameInputField.text) || string.IsNullOrEmpty(secondNameInputField.text))
        {
            Debug.Log("Name fields cannot be empty.");
            return;
        }
        if (string.IsNullOrEmpty(signUpEmailInputField.text))
        {
            Debug.Log("Email field cannot be empty.");
            return;
        }
        if (!ValidateEmail(signUpEmailInputField.text))
        {
            Debug.Log("Invalid email format.");
            return;
        }
        if (string.IsNullOrEmpty(contactInputField.text))
        {
            Debug.Log("Contact number cannot be empty.");
            return;
        }
        if (string.IsNullOrEmpty(signUpPasswordInputField.text))
        {
            Debug.Log("Password cannot be empty.");
            return;
        }
        if (string.IsNullOrEmpty(confirmPasswordInputField.text))
        {
            Debug.Log("Confirm Password cannot be empty.");
            return;
        }
        if (signUpPasswordInputField.text != confirmPasswordInputField.text)
        {
            Debug.Log("Passwords do not match.");
            return;
        }
        if (signUpPasswordInputField.text.Length < 8)
        {
            Debug.Log("Password must be at least 8 characters long.");
            return;
        }

        AuthManager.instance.RegisterUser(
            firstNameInputField.text,
            secondNameInputField.text,
            signUpEmailInputField.text.ToLower(),
            contactInputField.text,
            signUpPasswordInputField.text,
            confirmPasswordInputField.text
        );
    }


    private bool ValidateEmail(string email)
    {
        return !string.IsNullOrEmpty(email) && Regex.IsMatch(email, MatchEmailPattern);
    }
}
