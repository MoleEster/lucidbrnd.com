const popupLinks = document.querySelectorAll('.product-card');
const body = document.querySelector('body');    
const lockPadding = document.querySelectorAll(".lock-padding");
const popupName = "popup"

let unlock = true;

const timeout = 800;

if (popupLinks.length > 0)
{
    for (let index = 0; index < popupLinks.length; index++)
    {
        const popupLink = popupLinks[index];
        popupLink.addEventListener("click", function (e)
        {
            const currentPopup = document.getElementById(popupName);
            popupOpen(currentPopup);
            e.preventDefault();
        })
    }
}

const popupCloseIcon = document.querySelectorAll('.close_popup');

if (popupCloseIcon.length > 0)
{
    for (let index = 0; index < popupCloseIcon.length; index++)
    {
        const el = popupCloseIcon[index];
        el.addEventListener('click', function (e)
        {
            popupClose(el.closest('.popup'));
            e.preventDefault();
        })
    }
}

function popupOpen(currentPopup)
{
    if (currentPopup && unlock)
    {
        const popupActive = document.querySelector(".popup.open");
        if (popupActive) {
            popupClose(popupActive, false);
        }
        else {
            bodyLock();
        }
        currentPopup.classList.add('open');

        const children = currentPopup.querySelectorAll(".popup-close");

        for (let index = 0; index < children.length; index++)
        {
            const el = children[index];
            el.addEventListener("click", function (e)
            {
                popupClose(document.querySelector(".popup.open"));
            })

        }
    }
}

function popupClose(currentPopup, doUnlock = true)
{
    if (unlock) {
        currentPopup.classList.remove('open');
        if (doUnlock) {
            bodyUnlock();
        }
    }
}

function bodyLock(){        
    const lockPaddingValue = window.innerWidth - document.querySelector("body").offcetWidth + 'px';

    if (lockPadding.length > 0) {
        for (let index = 0; index <lockPadding.length; index++) {
            const el = lockPadding[index];
            el.style.paddingRight = lockPaddingValue;
        }
    }
    body.style.paddingRight = lockPaddingValue
    body.classList.add('lock');
   
    unlock = false;
    setTimeout(function () {
        unlock = true;
    }, timeout);
}

function bodyUnlock() {
    setTimeout(function () {
        if (lockPadding.length > 0) {
            for (let index = 0; index < lockPadding.length; index++) {
                const el = lockPadding[index];
                el.style.paddingRight = '0px';
            }
        }
        body.style.paddingRight = '0px';
        body.classList.remove('lock');
    }, timeout);

    unlock = false;
    setTimeout(function () {
        unlock = true;
    }, timeout);
}

jQuery(document).ready(function () {
        $('.product-card').click(function (e) {
            var product = $(this).data("series");
            $(".popup-content").load("/Home/GetProduct", { ProductId: product });
        })
});