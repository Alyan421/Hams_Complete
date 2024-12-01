namespace HMS_Final.Models
{
    public class User : BaseEntity<int>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public long OTP { get; set; } // One-Time Password
        public DateTime? OTPExpiry { get; set; } // When the OTP will expire
        public bool IsVerified { get; set; } = false; // Verification status
        public int ResendCount { get; set; } = 0; // Limit resend attempts

        // Many-to-Many relationship with hospitals
        public ICollection<UserHospital> UserHospitals { get; set; }

        // One-to-Many relationship with appointments
        public ICollection<Appointment> Appointments { get; set; }

        public void GenerateOTP()
        {
            Random random = new Random();
            this.OTP = random.Next(100000, 999999); // Generate a 6-digit OTP
            this.OTPExpiry = DateTime.Now.AddMinutes(1); // Set expiry time to 5 minutes from now
        }

        public bool VerifyOTP(long otp)
        {
            if (this.OTP == otp && this.OTPExpiry > DateTime.Now)
            {
                this.IsVerified = true;
                return true;
            }
            return false;
        }
    }
}
