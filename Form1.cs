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
            InitializeComponent(); // Стандартная инициализация формы Windows Forms
            SetupUI(); // Создание всех элементов управления
        }

        // Все элементы управления создаются программно, не через дизайнер
        // Динамически создается пользовательский интерфейс игры
        private void SetupUI()
        {
            this.Text = "Виселица";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Кнопка для первого игрока (загадывающего)
            Button btnSetWord = new Button
            {
                Text = "Загадать слово",
                Location = new Point(10, 10),
                Size = new Size(120, 30)
            };
            btnSetWord.Click += BtnSetWord_Click; // Подключаем обработчик события

            // Поле для отображения текущего состояния слова 
            Label lblWord = new Label
            {
                Name = "lblWord", // Имя для последующего доступа через Controls["lblWord"]
                Location = new Point(10, 50),
                Size = new Size(560, 40),
                Font = new Font("Arial", 24, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Поле для отображения использованных букв
            Label lblUsed = new Label
            {
                Name = "lblUsed",
                Location = new Point(10, 100),
                Size = new Size(560, 25),
                Font = new Font("Arial", 12),
                Text = "Использованные буквы: ",
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Информационная метка с инструкциями для второго игрока
            Label lblInfo = new Label
            {
                Name = "lblInfo",
                Location = new Point(10, 130),
                Size = new Size(560, 25),
                Font = new Font("Arial", 12),
                Text = "Игрок 2, угадай слово! Введи букву в поле и нажми «Проверить».",
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Текстовое поле для ввода буквы
            TextBox txtInput = new TextBox
            {
                Name = "txtInput",
                Location = new Point(200, 165),
                Size = new Size(40, 30),
                MaxLength = 1, // Разрешаем только один символ
                Font = new Font("Arial", 16)
            };
            txtInput.KeyDown += TxtInput_KeyDown; // Обработка нажатия Enter

            // Кнопка для проверки введенной буквы
            Button btnCheck = new Button
            {
                Text = "Проверить букву",
                Location = new Point(250, 165),
                Size = new Size(120, 30)
            };
            btnCheck.Click += BtnCheck_Click;

            // PictureBox для рисования виселицы и человечка
            PictureBox pictureBox = new PictureBox
            {
                Name = "pictureBox",
                Location = new Point(150, 200),
                Size = new Size(300, 250),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            pictureBox.Paint += PictureBox_Paint; // Обработчик события перерисовки

            // Добавляем все элементы управления на форму
            Controls.AddRange(new Control[]
            {
                btnSetWord, lblWord, lblUsed, lblInfo, txtInput, btnCheck, pictureBox
            });
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

            // Метка с инструкцией
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
                    textBox.Focus();
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
                ((PictureBox)Controls["pictureBox"]).Refresh(); // Перерисовываем виселицу
                Controls["txtInput"].Focus(); // Устанавливаем фокус на поле ввода
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
            // Получаем ссылку на текстовое поле ввода
            TextBox txt = (TextBox)Controls["txtInput"];
            string input = txt.Text.ToUpper().Trim(); // Приводим к верхнему регистру

            if (input.Length == 0) return; // Если поле пустое - ничего не делаем
            char letter = input[0]; // Берем первый символ

            // Проверка, что введена буква (дополнительная проверка)
            if (!char.IsLetter(letter))
            {
                SystemSounds.Beep.Play(); // Звуковое уведомление об ошибке
                MessageBox.Show("Введите букву!");
                txt.Clear();
                return;
            }

            // Проверка, что буква еще не использовалась
            if (usedLetters.Contains(letter))
            {
                SystemSounds.Beep.Play();
                MessageBox.Show("Буква уже использована!");
                txt.Clear();
                return;
            }

            // Добавляем букву в использованные
            usedLetters.Add(letter);
            txt.Clear(); // Очищаем поле ввода

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

            UpdateDisplay(); // Обновляем интерфейс

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

            ((PictureBox)Controls["pictureBox"]).Refresh(); // Перерисовываем виселицу
        }

        // Обновление отображения состояния игры
        // Обновляет метки с угаданным словом и использованными буквами
        private void UpdateDisplay()
        {
            // Преобразуем строку в массив символов и соединяем с пробелами
            ((Label)Controls["lblWord"]).Text = string.Join(" ", guessedWord.ToCharArray());

            // Отображаем использованные буквы в алфавитном порядке
            ((Label)Controls["lblUsed"]).Text = "Использованные буквы: " +
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
            ((Label)Controls["lblWord"]).Text = "";
            ((Label)Controls["lblUsed"]).Text = "Использованные буквы: ";
            ((PictureBox)Controls["pictureBox"]).Refresh(); // Очищаем рисунок виселицы
        }

        // Отрисовка виселицы и человечка
        // Вызывается при каждой перерисовке PictureBox
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // Сглаживание линий

            // Рисуем основу виселицы (всегда видна)
            g.DrawLine(Pens.Black, 50, 230, 250, 230); // Основание (земля)
            g.DrawLine(Pens.Black, 100, 230, 100, 40);  // Столб (вертикальная часть)
            g.DrawLine(Pens.Black, 100, 40, 200, 40);   // Верхняя перекладина
            g.DrawLine(Pens.Black, 200, 40, 200, 60);   // Верёвка

            // Рисуем части человечка в зависимости от количества ошибок
            if (wrongAttempts >= 1) g.DrawEllipse(Pens.Black, 185, 60, 30, 30);      // Голова
            if (wrongAttempts >= 2) g.DrawLine(Pens.Black, 200, 90, 200, 140);       // Тело
            if (wrongAttempts >= 3) g.DrawLine(Pens.Black, 200, 100, 180, 120);      // Левая рука
            if (wrongAttempts >= 4) g.DrawLine(Pens.Black, 200, 100, 220, 120);      // Правая рука
            if (wrongAttempts >= 5) g.DrawLine(Pens.Black, 200, 140, 185, 170);      // Левая нога
            if (wrongAttempts >= 6) g.DrawLine(Pens.Black, 200, 140, 215, 170);      // Правая нога
        }
    }
}