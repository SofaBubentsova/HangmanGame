namespace HangmanGame
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSetWord = new System.Windows.Forms.Button();
            this.lblWord = new System.Windows.Forms.Label();
            this.lblUsed = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnCheck = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSetWord
            // 
            this.btnSetWord.Location = new System.Drawing.Point(10, 10);
            this.btnSetWord.Name = "btnSetWord";
            this.btnSetWord.Size = new System.Drawing.Size(120, 30);
            this.btnSetWord.TabIndex = 0;
            this.btnSetWord.Text = "Загадать слово";
            this.btnSetWord.UseVisualStyleBackColor = true;
            this.btnSetWord.Click += new System.EventHandler(this.BtnSetWord_Click);
            // 
            // lblWord
            // 
            this.lblWord.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWord.Location = new System.Drawing.Point(10, 50);
            this.lblWord.Name = "lblWord";
            this.lblWord.Size = new System.Drawing.Size(560, 40);
            this.lblWord.TabIndex = 1;
            this.lblWord.Text = "";
            this.lblWord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUsed
            // 
            this.lblUsed.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUsed.Location = new System.Drawing.Point(10, 100);
            this.lblUsed.Name = "lblUsed";
            this.lblUsed.Size = new System.Drawing.Size(560, 25);
            this.lblUsed.TabIndex = 2;
            this.lblUsed.Text = "Использованные буквы: ";
            this.lblUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInfo
            // 
            this.lblInfo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblInfo.Location = new System.Drawing.Point(10, 130);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(560, 25);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "Игрок 2, угадай слово! Введи букву в поле и нажми «Проверить».";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtInput.Location = new System.Drawing.Point(200, 165);
            this.txtInput.MaxLength = 1;
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(40, 32);
            this.txtInput.TabIndex = 4;
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtInput_KeyDown);
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(150, 200);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(300, 250);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox_Paint);
            // 
            // btnCheck
            // 
            this.btnCheck.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCheck.Location = new System.Drawing.Point(250, 165);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(120, 30);
            this.btnCheck.TabIndex = 6;
            this.btnCheck.Text = "Проверить букву";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.BtnCheck_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 480);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblUsed);
            this.Controls.Add(this.lblWord);
            this.Controls.Add(this.btnSetWord);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSetWord;
        private System.Windows.Forms.Label lblWord;
        private System.Windows.Forms.Label lblUsed;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnCheck;
    }
}

