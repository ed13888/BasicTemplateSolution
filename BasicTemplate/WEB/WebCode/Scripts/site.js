

function setGoTop() {
    var scrH = document.documentElement.scrollTop + document.body.scrollTop;
    if (scrH > 200) {
        $('#gotop').fadeIn(400);
    } else {
        $('#gotop').stop().fadeOut(400);
    }
}
function autoPlay() {
    var myAuto = document.getElementById('audio');
    myAuto.play();
}
function closePlay() {
    var myAuto = document.getElementById('audio');
    myAuto.pause();
    myAuto.load();
}
function showtime() {
    var now_time = new Date();  // 创建时间对象
    var year = now_time.getFullYear();
    var month = now_time.getMonth() + 1;
    var day = now_time.getDay();
    var hours = now_time.getHours(); //获得当前小时数
    var minutes = now_time.getMinutes(); //获得当前分钟数
    var seconds = now_time.getSeconds(); //获得当前秒数
    var timer = year + "-" + month + "-" + day + " " + ((hours > 12) ? hours - 12 : hours); //将小时数值赋予变量timer
    timer += ((minutes < 10) ? ":0" : ":") + minutes; //将分钟数值赋予变量timer
    timer += ((seconds < 10) ? ":0" : ":") + seconds; //将秒数值赋予变量timer
    timer += " " + ((hours > 12) ? "pm" : "am"); //将字符am或pm赋予变量timer
    $("#footer_time").html(timer); //在名为clock的表单中输出变量timer的值
    setTimeout("showtime()", 1000); //设置每隔一秒钟自动调用一次showtime()函数

}


function refreshFooterNav() {
    var url = "@requestUrl";
    if (url == "")
        return;

    var arr = $("#footer_nav li a").map(function (i, v) {
        return v.attributes["href"].nodeValue;
    });

    $.each(arr, function (i, v) {
        if (url.indexOf(v) != -1) {
            $("#footer_nav li a[href$='" + v + "']:eq(0)").attr("href", "javascript:void(0)").parent().addClass("active");
            return false;
        }
    });
}


function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]);
    return null;
}

function getCookie(sName) {
    var aCookie = document.cookie.split("; ");
    for (var i = 0; i < aCookie.length; i++) {
        var aCrumb = aCookie[i].split("=");
        if (sName == aCrumb[0])
            return unescape(decodeURIComponent(aCrumb[1]));
    }
    return null;
}


function setLoading(val) {
    val = val || 0;
    var progressBar = $("#loadingModal .progress-bar");
    if (val >= 100) {
        //$("#progress").fadeOut(100);
        $("#loadingModal").modal('hide');
    }
    else {
        progressBar.animate({ width: (val || 0) + '%' }, 100);
    }
}

function loadSignalR(func) {

    var conn = $.connection("/myPath");

    conn.start().done(function (data) {
        var content = $("#info").val();
        var name = getCookie("connectionName");
        $("#info").val("连接成功，您的名片为： " + name + "\r\n" + content);
    });

    conn.received(function (data) {
        var content = $("#info").val();
        $("#info").val(data + "\r\n" + content);
    });

    function sendMsg() {
        var val = $('#msg').val();
        if (val != "") {
            conn.send(val);
            $('#msg').val("");
        }
    }
}