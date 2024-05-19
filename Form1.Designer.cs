
namespace Translator
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
            this.openButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.sourceTextBox = new System.Windows.Forms.RichTextBox();
            this.resultTextBox = new System.Windows.Forms.RichTextBox();
            this.exprTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(1392, 15);
            this.openButton.Margin = new System.Windows.Forms.Padding(4);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(205, 71);
            this.openButton.TabIndex = 2;
            this.openButton.Text = "Открыть";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(1392, 94);
            this.runButton.Margin = new System.Windows.Forms.Padding(4);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(205, 71);
            this.runButton.TabIndex = 3;
            this.runButton.Text = "Запустить";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.Location = new System.Drawing.Point(16, 15);
            this.sourceTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.Size = new System.Drawing.Size(1367, 530);
            this.sourceTextBox.TabIndex = 4;
            this.sourceTextBox.Text = "";
            // 
            // resultTextBox
            // 
            this.resultTextBox.Location = new System.Drawing.Point(16, 553);
            this.resultTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.Size = new System.Drawing.Size(694, 210);
            this.resultTextBox.TabIndex = 5;
            this.resultTextBox.Text = "";
            this.resultTextBox.UseWaitCursor = true;
            // 
            // exprTextBox
            // 
            this.exprTextBox.Location = new System.Drawing.Point(717, 553);
            this.exprTextBox.Name = "exprTextBox";
            this.exprTextBox.Size = new System.Drawing.Size(666, 210);
            this.exprTextBox.TabIndex = 6;
            this.exprTextBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1613, 778);
            this.Controls.Add(this.exprTextBox);
            this.Controls.Add(this.resultTextBox);
            this.Controls.Add(this.sourceTextBox);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.openButton);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Translator";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.RichTextBox sourceTextBox;
        private System.Windows.Forms.RichTextBox resultTextBox;
        private System.Windows.Forms.RichTextBox exprTextBox;
    }
}