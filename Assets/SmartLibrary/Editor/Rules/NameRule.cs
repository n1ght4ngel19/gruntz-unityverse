using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Bewildered.SmartLibrary
{
    /// <summary>
    /// Compares an object's name against a string using a matching option.
    /// </summary>
    [Serializable]
    public class NameRule : LibraryRuleBase
    {
        public enum NameMatchType { StartsWith, Contains, Regex }

        [SerializeField] private NameMatchType _matchType;
        [SerializeField] private string _text;
      
        /// <summary>
        /// The text string to match with an object's name.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set  { _text = value; }
        }

        /// <summary>
        /// The type of matching on an object's name to perform.
        /// </summary>
        public NameMatchType MatchType
        {
            get { return _matchType; }
            set { _matchType = value; }
        }

        public override string InvalidSettingsWarning
        {
            get { return "At least two characters are required."; }
        }

        /// <inheritdoc/>
        public override string SearchQuery
        {
            get
            {
                // Name Regex:  \/TEXT.[^\/]+$
                // Contains Regex: // .+TEXT[^/]+$

                // Need to have the filter within quotes because if there is a space in the filter text
                // it will no longer check if it starts exactly with the entered text. Example: "name:my material" would also return "mytexture".
                switch (_matchType)
                {
                    case NameMatchType.StartsWith:
                        return $@"{'"'}\/{_text}.[^\/]+${'"'}";
                    case NameMatchType.Contains:
                        return $"{'"'}.+{_text}[^/]+${'"'}"; 
                    case NameMatchType.Regex:
                        return $"{'"'}{_text}{'"'}";
                    default:
                        return string.Empty;
                }
            }
        }

        /// <inheritdoc/>
        public override bool Matches(LibraryItem item)
        {
            switch (_matchType)
            {
                case NameMatchType.StartsWith:
                    return item.Name.StartsWith(_text, StringComparison.InvariantCultureIgnoreCase);
                case NameMatchType.Contains:
                    return item.Name.IndexOf(_text, StringComparison.InvariantCultureIgnoreCase) > -1;
                case NameMatchType.Regex:
                    return Regex.IsMatch(item.Name, _text, RegexOptions.IgnoreCase);
            }

            return false;
        }
        
        public override bool IsRuleValid()
        {
            return !string.IsNullOrWhiteSpace(_text) && _text.Length >= 2;
        }
    } 
}
