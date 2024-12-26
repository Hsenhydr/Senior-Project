window.onscroll = function () {
    const backToTopButton = document.querySelector('.arrowup');
    if (document.body.scrollTop > 200 || document.documentElement.scrollTop > 200) {
        backToTopButton.style.display = "block";
    } else {
        backToTopButton.style.display = "none";
    }
};

document.querySelector('.arrowup').addEventListener('click', function (e) {
    e.preventDefault();
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
});


const container = document.getElementById('container1');
const registerBtn = document.getElementById('register');
const loginBtn = document.getElementById('login');



registerBtn.addEventListener('click', () => {
    container.classList.add("active");
});

loginBtn.addEventListener('click', () => {
    container.classList.remove("active");
});

//////


document.getElementById('signup').addEventListener('submit', function (event) {
    event.preventDefault();

    const errors = [];

    // Validate National ID (12 digits)
    const nationalid = document.getElementById('nationalid').value;

    // Check if the National ID is 12 digits, a number, and not negative
    if (nationalid.length !== 12 || isNaN(nationalid) || parseInt(nationalid) < 0) {
        errors.push('National ID must be a 12-digit positive number.');
    }

    // Validate Area Code (Not empty)
    const areacode = document.getElementById('areacode').value;
    if (!areacode) {
        errors.push('Area Code is required.');
    }

    // Validate First and Last Name (Not empty)
    const fname = document.querySelector('input[name="fname"]').value;
    const lname = document.querySelector('input[name="lname"]').value;
    if (!fname || !lname) {
        errors.push('Both First Name and Last Name are required.');
    }

    // Validate Email
    const email = document.querySelector('input[name="email"]').value;
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    if (!emailRegex.test(email)) {
        errors.push('Enter a valid email address.');
    }

    // Validate Phone Number (8 digits)
    const phn = document.querySelector('input[name="phn"]').value;
    if (phn.length !== 8 || isNaN(phn)) {
        errors.push('Phone number must be 8 digits.');
    }

    // Validate Password (at least 8 characters, contains both letters and numbers)
    const pass = document.querySelector('input[name="pass"]').value;
    const confirmpass = document.querySelector('input[name="confirmpass"]').value;
    const passwordRegex = /^(?=.*[a-zA-Z])(?=.*\d)[A-Za-z\d]{8,}$/;
    if (!passwordRegex.test(pass)) {
        errors.push('Password must be at least 8 characters long and contain both letters and numbers.');
    }

    // Check if password and confirm password match
    if (pass !== confirmpass) {
        errors.push('Passwords do not match.');
    }

    if (errors.length > 0) {
        let errorList = '';
        errors.forEach(error => {
            errorList += `<li>${error}</li>`;
        });

        // Show SweetAlert with errors in a ul
        Swal.fire({
            icon: 'error',
            title: 'Validation Errors',
            html: `<ul>${errorList}</ul>`,
            confirmButtonText: 'Okay'
        });
    } else {
        // Submit the form if no errors
        this.submit();
    }
});