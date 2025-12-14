using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace HangmanGame
{
    public partial class Form1 : Form
    {
        // Объект для отрисовки виселицы
        private HangmanDrawer hangmanDrawer;

        // Хранит загаданное слово в верхнем регистре
        private string secretWord = "";

        // Отображает текущее состояние угаданного слова (например, "_ _ _ _")
        private string guessedWord = "";

        // Множество для хранения уже использованных букв (HashSet - не хранит дубликаты)
        private HashSet<char> usedLetters = new HashSet<char>();

        // Счетчик неправильных попыток
        private int wrongAttempts = 0;

        // Максимальное количество ошибок до проигрыша
        private const int MAX_WRONG = 6; // Части виселицы: голова, тело, 2 руки, 2 ноги

        public Form1()
        {
            InitializeComponent(); // Загружает интерфейс из Form1.Designer.cs
            hangmanDrawer = new HangmanDrawer(); // Создаем отдельный объект для рисования
        }

        // Обработчик нажатия кнопки "Загадать слово"
        // Создает диалоговое окно для ввода слова первым игроком
        private void BtnSetWord_Click(object sender, EventArgs e)
        {
            // Создаём временную форму для ввода слова
            var inputForm = new Form();
            string enteredWord = null; // Переменная для хранения введенного слова

            // Настройка формы ввода
            inputForm.Text = "Загадать слово";
            inputForm.Size = new Size(350, 150);
            inputForm.StartPosition = FormStartPosition.CenterParent;
            inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputForm.MaximizeBox = false;
            inputForm.MinimizeBox = false;
            inputForm.ShowInTaskbar = false;

            // Метка с инструкцией для первого игрока
            var label = new Label
            {
                Text = "Игрок 1, введите слово (2–15 букв, только буквы, без пробелов):",
                Location = new Point(12, 12),
                Size = new Size(300, 40),
                AutoSize = false
            };

            // Текстовое поле для ввода слова
            var textBox = new TextBox
            {
                Location = new Point(12, 55),
                Size = new Size(200, 23)
            };

            // Обработчик события KeyPress ограничивает ввод только буквами
            textBox.KeyPress += (s, ke) =>
            {
                // Разрешаем только буквы и управляющие символы (Backspace, Delete)
                if (!char.IsLetter(ke.KeyChar) && !char.IsControl(ke.KeyChar))
                    ke.Handled = true; // Игнорируем небуквенные символы
            };

            // Кнопка OK для подтверждения ввода
            var okButton = new Button
            {
                Text = "OK",
                Location = new Point(220, 53),
                Size = new Size(75, 25)
            };
            okButton.Click += (s, ev) =>
            {
                string input = textBox.Text.Trim();

                // Проверка на пустой ввод
                if (string.IsNullOrEmpty(input))
                {
                    MessageBox.Show("Слово не может быть пустым.");
                    textBox.Focus(); // Возвращаем фокус на поле ввода
                    return;
                }

                // Проверка длины слова
                if (input.Length < 2 || input.Length > 15)
                {
                    MessageBox.Show("Слово должно содержать от 2 до 15 букв!");
                    textBox.Focus();
                    return;
                }

                // Проверка, что все символы - буквы
                if (!input.All(char.IsLetter))
                {
                    MessageBox.Show("Слово должно содержать только буквы!");
                    textBox.Focus();
                    return;
                }

                enteredWord = input;
                inputForm.DialogResult = DialogResult.OK;
                inputForm.Close();
            };

            // Добавляем элементы управления на форму ввода
            inputForm.Controls.AddRange(new Control[] { label, textBox, okButton });
            inputForm.AcceptButton = okButton; // Назначаем кнопку OK кнопкой по умолчанию
            inputForm.ActiveControl = textBox; // Устанавливаем фокус на текстовое поле

            // Показываем форму ввода как модальное диалоговое окно
            if (inputForm.ShowDialog(this) == DialogResult.OK)
            {
                // Инициализация игры с новым словом
                secretWord = enteredWord.ToUpper(); // Приводим к верхнему регистру для единообразия
                guessedWord = new string('_', secretWord.Length); // Создаем строку из подчеркиваний
                usedLetters.Clear(); // Очищаем список использованных букв
                wrongAttempts = 0; // Сбрасываем счетчик ошибок

                UpdateDisplay(); // Обновляем интерфейс
                pictureBox.Refresh(); // Перерисовываем виселицу (сброс на начальное состояние)
                txtInput.Focus(); // Устанавливаем фокус на поле ввода для удобства игрока
            }
        }

        // Обработчик нажатия кнопки "Проверить букву"
        // Вызывает основную логику проверки введенной буквы
        private void BtnCheck_Click(object sender, EventArgs e)
        {
            ProcessGuess();
        }

        // Обработчик нажатия клавиши Enter в текстовом поле
        // Позволяет проверять букву без необходимости нажимать кнопку мышью
        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ProcessGuess();
            }
        }

        // Основная логика обработки угадывания буквы
        // Проверяет введенную букву и обновляет состояние игры
        private void ProcessGuess()
        {
            string input = txtInput.Text.ToUpper().Trim(); // Приводим к верхнему регистру

            if (input.Length == 0) return; // Если поле пустое - ничего не делаем
            char letter = input[0]; // Берем первый символ

            // Проверка, что введена буква (дополнительная проверка)
            if (!char.IsLetter(letter))
            {
                SystemSounds.Beep.Play(); // Звуковое уведомление об ошибке
                MessageBox.Show("Введите букву!");
                txtInput.Clear(); // Очищаем поле для нового ввода
                return;
            }

            // Проверка, что буква еще не использовалась
            if (usedLetters.Contains(letter))
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Буква уже использована!");
                txtInput.Clear();
                return;
            }

            // Добавляем букву в использованные
            usedLetters.Add(letter);
            txtInput.Clear(); // Очищаем поле ввода для следующей буквы

            // Проверяем, есть ли буква в загаданном слове
            if (secretWord.Contains(letter))
            {
                // Буква угадана правильно - обновляем отгаданное слово
                var guessedArray = guessedWord.ToCharArray();
                for (int i = 0; i < secretWord.Length; i++)
                {
                    if (secretWord[i] == letter)
                        guessedArray[i] = letter; // Заменяем подчеркивание угаданной буквой
                }
                guessedWord = new string(guessedArray);
            }
            else
            {
                wrongAttempts++; // Увеличиваем счетчик ошибок
                SystemSounds.Beep.Play(); // Сигнал об ошибке
            }

            UpdateDisplay(); // Обновляем интерфейс (слово и список букв)

            // Проверка условий победы/поражения
            if (guessedWord == secretWord)
            {
                MessageBox.Show("Поздравляем! Слово угадано!", "Победа!");
                ResetGame(); // Сбрасываем игру для новой партии
            }
            else if (wrongAttempts >= MAX_WRONG)
            {
                MessageBox.Show($"Проигрыш! Слово было: {secretWord}", "Игра окончена");
                ResetGame();
            }

            pictureBox.Refresh();  // Перерисовываем виселицу
        }

        // Обновление отображения состояния игры
        // Обновляет метки с угаданным словом и использованными буквами
        private void UpdateDisplay()
        {
            // Преобразуем строку в массив символов и соединяем с пробелами для читаемости
            lblWord.Text = string.Join(" ", guessedWord.ToCharArray());

            // Отображаем использованные буквы в алфавитном порядке через запятую
            lblUsed.Text = "Использованные буквы: " +
                string.Join(", ", usedLetters.OrderBy(c => c));
        }

        // Сброс игры в начальное состояние
        // Очищает все данные игры и готовит интерфейс к новой игре
        private void ResetGame()
        {
            secretWord = "";
            guessedWord = "";
            usedLetters.Clear();
            wrongAttempts = 0;
            lblWord.Text = "";
            lblUsed.Text = "Использованные буквы: ";
            pictureBox.Refresh(); // Перерисовывает пустую виселицу
        }

        // Обработчик события перерисовки PictureBox (отрисовка виселицы и человечка)
        // Отрисовка виселицы происходит в отдельном классе HangmanDrawer
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            hangmanDrawer.Draw(e.Graphics, wrongAttempts);
        }     
               
    }
}