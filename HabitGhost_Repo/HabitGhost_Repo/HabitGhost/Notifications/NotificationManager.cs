using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HabitGhost.Notifications
{
    public static class NotificationManager
    {
        private static bool _notificationsEnabled = true;
        private static List<NotificationForm> _notificationStack = new List<NotificationForm>();
        private static HideAllNotificationsForm _hideAllForm;

        private static readonly string notificationsFilePath = Path.Combine(Application.StartupPath, "notifications.json");
        private static List<NotificationData> _notificationHistory = new List<NotificationData>();

        private static void PositionNotifications()
        {
            var screenBounds = Screen.PrimaryScreen.WorkingArea;
            int yOffset = screenBounds.Bottom - 8;

            if (_notificationStack.Count >= 2)
            {
                _hideAllForm?.CloseForm();
                _hideAllForm = new HideAllNotificationsForm();
                _hideAllForm.ShowForm();
            }
            else if (_notificationStack.Count < 2 && _hideAllForm != null)
            {
                _hideAllForm.Close();
                _hideAllForm = null;
            }

            for (int i = _notificationStack.Count - 1; i >= 0; i--)
            {
                var notification = _notificationStack[i];
                notification.Location = new Point(screenBounds.Right - notification.Width - 8, yOffset - notification.Height);
                yOffset -= notification.Height + 4;
            }

            if (_hideAllForm != null) _hideAllForm.Location = new Point(screenBounds.Right - _hideAllForm.Width - 8, yOffset - _hideAllForm.Height - 4);
        }

        public static void RemoveNotification(NotificationForm notification)
        {
            if (_notificationStack.Contains(notification))
            {
                _notificationStack.Remove(notification);
                PositionNotifications();
            }
            if (_notificationStack.Count < 2) _hideAllForm?.CloseForm();
        }
        public static void RemoveNotificationFromHistory(string message)
        {
            var notification = _notificationHistory.FirstOrDefault(n => n.Message == message);
            if (notification != null) _notificationHistory.Remove(notification);
            SaveNotificationsToFile();
        }
        public static void AddNotification(string message, MainForm mainForm)
        {
            if (!_notificationsEnabled) return;
            NotificationForm notification = new NotificationForm(message, mainForm);
            _notificationStack.Add(notification);
            if (mainForm != null)
            {
                _notificationHistory.Add(new NotificationData { Message = message, Timestamp = DateTime.Now });
                SaveNotificationsToFile();
            }
            PositionNotifications();
            notification.ShowNotification();
        }

        public static void HideAllNotifications()
        {
            foreach (var notification in _notificationStack.ToList()) notification.Close();
            _notificationStack.Clear();
            if (_hideAllForm != null)
            {
                _hideAllForm.CloseForm();
                _hideAllForm = null;
            }
        }
        public static void ResetHideAllNotifications()
        {
            if (_hideAllForm != null) _hideAllForm = null;
        }

        public static string WrapText(string text, int maxWidth, Font font)
        {
            if (string.IsNullOrEmpty(text)) return text;

            StringBuilder wrappedText = new StringBuilder();
            string[] lines = text.Split(new[] { '\n' }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                string[] words = line.Split(' ');
                StringBuilder currentLine = new StringBuilder();

                foreach (string word in words)
                {
                    string wordToAdd = word;
                    if (currentLine.Length > 0)
                    {
                        wordToAdd = " " + word;
                    }

                    string testLine = currentLine.ToString() + wordToAdd;
                    Size size = TextRenderer.MeasureText(testLine, font);

                    if (size.Width <= maxWidth)
                    {
                        currentLine.Append(wordToAdd);
                    }
                    else
                    {
                        if (currentLine.Length > 0)
                        {
                            wrappedText.AppendLine(currentLine.ToString());
                            currentLine.Clear();
                        }

                        string remainingWord = word;
                        while (TextRenderer.MeasureText(remainingWord, font).Width > maxWidth)
                        {
                            int splitIndex = FindSplitIndex(remainingWord, maxWidth, font);
                            if (splitIndex > 0)
                            {
                                wrappedText.AppendLine(remainingWord.Substring(0, splitIndex));
                                remainingWord = remainingWord.Substring(splitIndex);
                            }
                            else
                            {
                                wrappedText.AppendLine(remainingWord[0].ToString());
                                remainingWord = remainingWord.Substring(1);
                            }
                        }
                        currentLine.Append(remainingWord);
                    }
                }

                if (currentLine.Length > 0)
                {
                    wrappedText.AppendLine(currentLine.ToString());
                }
            }

            return wrappedText.ToString().TrimEnd();
        }
        public static int FindSplitIndex(string text, int maxWidth, Font font)
        {
            int low = 0;
            int high = text.Length - 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;
                string testText = text.Substring(0, mid + 1);
                Size size = TextRenderer.MeasureText(testText, font);

                if (size.Width <= maxWidth)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return high;
        }

        private static void SaveNotificationsToFile()
        {
            try
            {
                File.WriteAllText(notificationsFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(_notificationHistory, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification("Error saving messages: " + ex.Message, null);
            }
        }

        public static void LoadNotificationsFromFile(MainForm mainForm)
        {
            if (!_notificationsEnabled) return;

            if (!File.Exists(notificationsFilePath)) return;

            try
            {
                string json = File.ReadAllText(notificationsFilePath);
                _notificationHistory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NotificationData>>(json) ?? new List<NotificationData>();

                foreach (var data in _notificationHistory)
                {
                    mainForm.UnreadNotifications.Add(new Notification { Message = data.Message, Timestamp = data.Timestamp });
                }

                PositionNotifications();
            }
            catch (Exception ex)
            {
                if (_notificationsEnabled)
                    AddNotification("Error loading messages: " + ex.Message, null);
            }
        }

        public static void SetNotificationsEnabled(bool enabled)
        {
            _notificationsEnabled = enabled;
            if (!enabled) HideAllNotifications();
        }
    }
}
