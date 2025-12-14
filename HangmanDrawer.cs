using System.Drawing;

namespace HangmanGame
{
    public class HangmanDrawer
    {
        /// <summary>
        /// Рисует виселицу и человечка в зависимости от количества ошибок
        /// </summary>
        /// <param name="graphics">Объект для рисования</param>
        /// <param name="wrongAttempts">Количество неправильных попыток</param>
        public void Draw(Graphics graphics, int wrongAttempts)
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Рисуем основу виселицы (всегда видна)
            DrawGallows(graphics);

            // Рисуем части человечка в зависимости от количества ошибок
            DrawHangman(graphics, wrongAttempts);
        }

        /// <summary>
        /// Рисует статичную основу виселицы
        /// </summary>
        private void DrawGallows(Graphics g)
        {
            g.DrawLine(Pens.Black, 50, 230, 250, 230); // Основание
            g.DrawLine(Pens.Black, 100, 230, 100, 40);  // Столб
            g.DrawLine(Pens.Black, 100, 40, 200, 40);   // Верхняя перекладина
            g.DrawLine(Pens.Black, 200, 40, 200, 60);   // Верёвка
        }

        /// <summary>
        /// Рисует части человечка
        /// </summary>
        private void DrawHangman(Graphics g, int wrongAttempts)
        {
            if (wrongAttempts >= 1) g.DrawEllipse(Pens.Black, 185, 60, 30, 30);      // Голова
            if (wrongAttempts >= 2) g.DrawLine(Pens.Black, 200, 90, 200, 140);       // Тело
            if (wrongAttempts >= 3) g.DrawLine(Pens.Black, 200, 100, 180, 120);      // Левая рука
            if (wrongAttempts >= 4) g.DrawLine(Pens.Black, 200, 100, 220, 120);      // Правая рука
            if (wrongAttempts >= 5) g.DrawLine(Pens.Black, 200, 140, 185, 170);      // Левая нога
            if (wrongAttempts >= 6) g.DrawLine(Pens.Black, 200, 140, 215, 170);      // Правая нога
        }
    }
}