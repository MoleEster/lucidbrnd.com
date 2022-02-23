const FIO = document.getElementById("FIOInformation-input");
const Email = document.getElementById("email-input");
const phone = document.getElementById("phone-input");
const adress = document.getElementById("adress-input");
const index = document.getElementById("index-input");
const button = document.getElementById("purchase-button");

jQuery(document).ready(function () {
button.addEventListener("click", function (event) {
        let good = true;
        if (FIO.value == "")
        {
            FIO.classList.add('invalid');
            good = false;
        }
        if (Email.value == "") {
            Email.classList.add('invalid');
            good = false;

        }
        if (phone.value == "") {
            phone.classList.add('invalid');
            good = false;

        }
        if (adress.value == "") {
            adress.classList.add('invalid');
            good = false;

        }
        if (index.value == "") {
            index.classList.add('invalid');
            good = false;

         }
    if (!good) event.preventDefault();

    });
});
