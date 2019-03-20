//上一页
function prevPage() {
    $(".section.active .section-content").addClass("animated").addClass("slideOutDown");

    setTimeout(function () {
        $(".section.active .section-content").removeClass("animated").removeClass("slideOutDown");
        $('#fullpage').fullpage.moveSectionUp();
    }, 1000);
}

//下一页
function nextPage() {
    $(".section.active .section-content").addClass("animated").addClass("slideOutUp");
    setTimeout(function () {
        $(".section.active .section-content").removeClass("animated").removeClass("slideOutUp");
        $('#fullpage').fullpage.moveSectionDown();
    }, 1000);
}