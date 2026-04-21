namespace StockBaseApp
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Empty InitializeComponent since UI is handled manually in LoginForm.cs
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // LoginForm
            // 
            ClientSize = new Size(282, 253);
            Name = "LoginForm";
            Text = "StockBase - Login";
            ResumeLayout(false);
        }
    }
}