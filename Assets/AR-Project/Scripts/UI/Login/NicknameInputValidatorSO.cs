using UnityEngine;

/// <summary>
/// Custom input validator for login, allows for this nickname format: *username*#*X-digits_number*
/// </summary>
[CreateAssetMenu(fileName = "New NicknameInputValidatorSO", menuName = "UI/Login/Nickname Input Validator")]
public class NicknameInputValidatorSO : TMPro.TMP_InputValidator
{
    /// <summary>
    /// Override Validate method to implement your own validation
    /// </summary>
    /// <param name="text">This is a reference pointer to the actual text in the input field; changes made to this text argument will also result in changes made to text shown in the input field</param>
    /// <param name="pos">This is a reference pointer to the input field's text insertion index position (your blinking caret cursor); changing this value will also change the index of the input field's insertion position</param>
    /// <param name="ch">This is the character being typed into the input field</param>
    /// <returns>Return the character you'd allow into </returns>
    public override char Validate(ref string text, ref int pos, char ch)
    {
        //Debug.Log($"Text = {text}; pos = {pos}; chr = {ch}");
        // If the text length is less than the max length
        if(text.Length < 30)
        {
            // If the text already contains an '#'..
            if (text.Contains('#'))
            {
                //..and the input character is a digit..
                if (ch >= '0' && ch <= '9')
                {
                    
                    // Insert the character at the given position if we're working in the Unity Editor
                    #if UNITY_EDITOR
                    text = text.Insert(pos, ch.ToString());
                    #endif
                    // Increment the insertion point by 1
                    pos++;
                    //.. then return the character
                    return ch;
                }
                // If not, then return null
                else
                {
                    return '\0';
                }
                
            }

            // If the character is a letter, a digit or a '#'
            if (char.IsLetterOrDigit(ch) || ch == '#')
            {
                // Insert the character at the given position if we're working in the Unity Editor
                #if UNITY_EDITOR
                text = text.Insert(pos, ch.ToString());
                #endif
                // Increment the insertion point by 1
                pos++;
                return ch;  
            }
            // If the character is not a number or a '#', return null
            else
            {
                return '\0';
            }
        }
        // If the text length is more or equal than the max length, return null
        else
        {
            return '\0';
        }              
    }
}