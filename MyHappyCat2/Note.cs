using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace MyHappyCAt
{

    public class UserNote
    {
        public int NoteId { get; set; }
        public string Content { get; set; }

        public Button DeleteButton { get; set; } // Button to delete the note
    }


    public class Note : PageItems
    {
        private string _inputContent;
        private int _noteId = 0;
        private Bitmap _noteBitmap;
        private List<UserNote> _usersNotes = new List<UserNote>();
        private Button _inputBox;
        private bool _isTyping = false;
        private Font _mainFont;
        private Color _textColor;
        private double _noteX; 
        private double _noteY; 
        private int _cursorBlinkCounter = 0; // Counter for cursor blinking
        private const int CursorBlinkRate = 30;
        private Rectangle _inputBoxRect;       
        private Color cursorColor = Color.Black; // Color of the cursor when typing

        public Note(double x, double y, Font mainFont, Color textColor)
        {
            _noteX = x;
            _noteY = y;
            _mainFont = mainFont;
            _textColor = textColor;            
            _noteBitmap = SplashKit.LoadBitmap("note", "note.png");
            _inputBox = new Button(275, 230, 190, 23, "inputBox.png");
            _inputBoxRect = SplashKit.RectangleFrom(280, 230, 190, 23); // Define the input box rectangle for mouse detection

            _noteId = 1;
            _inputContent = "";             
        }

        public void Draw()
        {
            SplashKit.DrawBitmap(_noteBitmap, _noteX, _noteY); // Draw the note image at position (10, 10)

            SplashKit.DrawText("Notes", _textColor, _mainFont, 16, _noteX + 90 - (SplashKit.TextWidth("Note", _mainFont, 16)/2), 25); // Draw the title of the notes section

            for (int i = 0; i < _usersNotes.Count; i++)
            {
                SplashKit.DrawText("- " + _usersNotes[i].Content, _textColor, _mainFont, 12, _noteX + 10, _noteY + 4 + SplashKit.TextHeight(_usersNotes[i].Content, _mainFont, 12) + i * 14);

                // Update button position for each note
                _usersNotes[i].DeleteButton.X = _noteX + 180;
                _usersNotes[i].DeleteButton.Y = _noteY + 5 + SplashKit.TextHeight(_usersNotes[i].Content, _mainFont, 12) + i * 14;
                _usersNotes[i].DeleteButton.Draw();
                
            }

            // Draw the input box for typing notes
            _inputBox.Draw();
            SplashKit.DrawText(_inputContent, Color.Black, _mainFont, 10, _inputBox.X + 7, _inputBox.Y + 3); // Draw the note content inside the input box
            // Draw the cursor if typing
            _cursorBlinkCounter++;
            if (_isTyping && (_cursorBlinkCounter / CursorBlinkRate) % 2 == 0)
            {
                SplashKit.DrawText("|", cursorColor, "MainFont", 14, _inputBox.X + 7 + SplashKit.TextWidth(_inputContent, _mainFont, 10), _inputBox.Y + 2);
            }

            if (_inputBox.IsClicked() && _usersNotes.Count >= 10)
            {
                SplashKit.DrawText("Not enough space", Color.Red, _mainFont, 12, _noteX + 10, _noteY + 200);
            }
            
        }

        public void Update()
        {

            // Update the note state if necessary
            // This could include checking for user input to edit the note.
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                if (_inputBox.IsMouseOver())
                {
                    if (!_isTyping)
                    {
                        _isTyping = true;
                        _inputContent = "";
                        SplashKit.StartReadingText(_inputBoxRect);
                    }
                }
                else
                {
                    if (_isTyping)
                    {
                        _isTyping = false;
                        SaveNote();
                        SplashKit.EndReadingText();
                        _inputContent = "";
                    }
                }
            }

            if (_isTyping && _usersNotes.Count <= 10)
            {
                HandleKeyboardInput();
            }

            for (int i = 0; i < _usersNotes.Count; i++)
            {
                if (_usersNotes[i].DeleteButton.IsClicked())
                {
                    Console.WriteLine($"Note with ID {_usersNotes[i].NoteId} was deleted.");
                    _usersNotes.RemoveAt(i); // Remove the note from the list                    
                    break; // Exit the loop after deleting a note
                }
            }


        }

        public void SaveNote()
        {
            if (string.IsNullOrEmpty(_inputContent))
            {
                Console.WriteLine("Empty note");
                return;
            }
            // Initial position, will be updated in Draw
            double btnX = _noteX + 50;
            double btnY = _noteY + 10 + SplashKit.TextHeight(_inputContent, _mainFont, 12) + (_usersNotes.Count * 15);

            UserNote note = new UserNote
            {
                NoteId = _noteId,
                Content = _inputContent,
                DeleteButton = new Button(btnX, btnY, 10, 12, "delete_note1.png", "delete_note2.png")
            };
            _usersNotes.Add(note);  //Add the note so it can appear on screen
            _noteId++;
            Console.WriteLine($"note '{note.Content}' (ID: {note.NoteId}) was saved.");
        }

        public void HandleKeyboardInput()
        {
            if (_inputContent.Length < 20)
            {
                cursorColor = Color.Black; // Default cursor color
                _inputContent = SplashKit.TextInput();
            }
            else
            {
                cursorColor = Color.Red; // Change cursor color to red if input exceeds limit
            }

            // Save note on Enter
            if (SplashKit.KeyTyped(KeyCode.ReturnKey))
            {
                SaveNote();
                _isTyping = false;
                SplashKit.EndReadingText();
                _inputContent = "";
            }
        }        
      
    }
}
