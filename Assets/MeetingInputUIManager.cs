using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeetingInputUIManager : MonoBehaviour
{
    public InputField titleInputField;
    public InputField descriptionInputField;
    public TextMeshProUGUI startDateText;
    public TextMeshProUGUI endDateText;
    public InputField summaryInputField;

    public Transform userScrollViewContent;
    public Transform agentScrollViewContent;

   

    public void ValidateAndCreateMeeting()
    {
        // Validate input fields
        if (string.IsNullOrEmpty(titleInputField.text))
        {
            Debug.Log("Title cannot be empty.");
            return;
        }

        if (string.IsNullOrEmpty(descriptionInputField.text))
        {
            Debug.Log("Description cannot be empty.");
            return;
        }

        if (string.IsNullOrEmpty(startDateText.text))
        {
            Debug.Log("Start date cannot be empty.");
            return;
        }

        if (string.IsNullOrEmpty(endDateText.text))
        {
            Debug.Log("End date cannot be empty.");
            return;
        }

        if (string.IsNullOrEmpty(summaryInputField.text))
        {
            Debug.Log("Summary cannot be empty.");
            return;
        }

        // Fetch users and agents, then create meeting
        FetchAndPassMeetingData((userIds, agentIds) =>
        {
            if (userIds.Count == 0)
            {
                Debug.Log("No users selected.");
                return;
            }

            if (agentIds.Count == 0)
            {
                Debug.Log("No agents selected.");
                return;
            }

            // Convert List<int> to int[] for compatibility with CreateMeeting
            int[] agentIdArray = agentIds.ToArray();
            int[] userIdArray = userIds.ToArray();

            // Call CreateMeeting from the CreateMeeting script
            Meeting_Calling.instance.CreateMeeting(
                titleInputField.text,
                descriptionInputField.text,
                startDateText.text,
                endDateText.text,
                summaryInputField.text,
                agentIdArray,
                userIdArray
            );
        });
    }

    private void FetchAndPassMeetingData(System.Action<List<int>, List<int>> callback)
    {
        List<int> userIds = new List<int>();
        List<int> agentIds = new List<int>();

        UserCalling.instance.GetUsers(users =>
        {
            foreach (var user in users)
            {
                if (IsUserSelected(user.first_name))
                {
                    userIds.Add(user.id);
                }
            }

            agentCalling.instance.GetAgent(agents =>
            {
                foreach (var agent in agents)
                {
                    if (IsAgentSelected(agent.name))
                    {
                        agentIds.Add(agent.id);
                    }
                }

                callback?.Invoke(userIds, agentIds);
            });
        });
    }

    private bool IsUserSelected(string userName)
    {
        foreach (Transform child in userScrollViewContent)
        {
            TMP_Text userText = child.GetComponentInChildren<TMP_Text>();
            if (userText != null && userText.text == userName)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsAgentSelected(string agentName)
    {
        foreach (Transform child in agentScrollViewContent)
        {
            TMP_Text agentText = child.GetComponentInChildren<TMP_Text>();
            if (agentText != null && agentText.text == agentName)
            {
                return true;
            }
        }
        return false;
    }
}
