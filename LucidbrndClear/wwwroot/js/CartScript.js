jQuery(".removing-from-cart").click(function () {
    var ProductId = $(this).data("series");
    var ProductSize = $(this).data("size");

    var ThisButtonsPanel = $(this).closest(".items-in-cart-buttons");
    var ThisUndoPanel = $(this).closest(".items-in-cart-buttons").siblings(".undo-panel-initial");
    var ThisInfoPanel = $(this).closest(".items-in-cart-buttons").siblings(".item-in-cart-information");
    var ThisButtonUndo = $(this).closest(".items-in-cart-buttons").siblings(".undo-panel-initial").children(".undo-delete-button-initial").children(".undo-delete-button");
    var ThisTimerPath = $(this).closest(".items-in-cart-buttons").siblings(".undo-panel-initial").children(".base-timer-initial").children(".base-timer").children(".base-timer__svg").children(".base-timer__circle").children(".base-timer-path-remaining");

    var thisItemInCart = $(this).closest(".items-in-cart-buttons").closest(".item-in-cart");
    ThisUndoPanel.addClass("visible");
    ThisInfoPanel.addClass("hidden");
    ThisButtonsPanel.addClass("hidden");

    startTimer(ThisTimerPath, 0, 283);
    var timer = setTimeout(function () {
        thisItemInCart.fadeOut();
        $(".items-in-cart-initial").load("/Cart/RemoveWholeProductInCart", { ProductId: ProductId, Size: ProductSize });
    }, 2000);

    ThisButtonUndo.click(function () {
        clearTimeout(timer);
        clearTimeout(startTimer);
        ThisUndoPanel.removeClass("visible");
        ThisInfoPanel.removeClass("hidden");
        ThisButtonsPanel.removeClass("hidden");
    });

});

function startTimer(timerPath, timePassed, dash_array) {
    var timeLeft;
    timerInterval = setInterval(() => {
        timePassed = timePassed += 1;
        timeLeft = 2 - timePassed;

        setCircleDasharray(timerPath, dash_array, timeLeft);
    }, 1000);
}


// Обновляем значение свойства dasharray, начиная с 283
function setCircleDasharray(timerPath, dash_array, timeLeft) {
    const circleDasharray = `${(
        calculateTimeFraction(timeLeft) * dash_array
    ).toFixed(0)} 283`;
    timerPath[0].setAttribute("stroke-dasharray", circleDasharray);
}


function calculateTimeFraction(timeLeft) {
    const rawTimeFraction = timeLeft / 2;
    return rawTimeFraction - (1 / 2) * (1 - rawTimeFraction)*2;
}




jQuery(".decrease-product-in-cart-button").click(function () {
    var ProductId = $(this).data("series");
    var ProductSize = $(this).data("size");
    $(".items-in-cart-initial").load("/Cart/RemoveFromCart", { ProductId: ProductId, Size: ProductSize });
    $(this).prop('disabled', true);
    setTimeout(function () {
        $(this).prop('disabled', false);
    }.bind(this), 1e3);
});



jQuery(".increase-product-in-cart-button").click(function () {
        var ProductId = $(this).data("series");
        var ProductSize = $(this).data("size");
        $(".items-in-cart-initial").load("/Cart/AddtoCart", { ProductId: ProductId, Size: ProductSize });
        $(this).prop('disabled', true);
        setTimeout(function () {
            $(this).prop('disabled', false);
        }.bind(this), 1e3);
});