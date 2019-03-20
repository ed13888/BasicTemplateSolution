

function setGoTop() {
    var scrH = document.documentElement.scrollTop + document.body.scrollTop;
    if (scrH > 200) {
        $('#gotop').fadeIn(400);
    } else {
        $('#gotop').stop().fadeOut(400);
    }
}
function goTop() {
    $('html,body').animate({ scrollTop: '0px' }, 200);
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
        //JQ方式
        //$("#loadingModal").modal('hide');
        //原生js方式
        hideLoading();
    }
    else {
        progressBar.animate({ width: (val || 0) + '%' }, 100);
    }
}

function showLoading() {
    document.body.classList.add("modal-open");
    document.getElementById("loadingModal").style.display = "block";
    document.getElementById("modalMask").style.display = "block";
}

function hideLoading() {
    //JQ方式
    //$('#loadingModal').modal({ backdrop: 'static', keyboard: false });
    //$("#loadingModal").modal('show');

    //原生js方式
    document.getElementById("loadingModal").style.display = "none";
    document.getElementById("modalMask").style.display = "none";
    $("body").removeClass("modal-open");
}


function loadSignalR() {

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

    $("#btnSendMsg").bind("click", sendMsg)
    $("#msg").bind("keydown", function (event) {
        if (event.keyCode == "13") {
            sendMsg();
        }
    })

    function sendMsg() {
        var val = $('#msg').val();
        if (val != "") {
            conn.send(val);
            $('#msg').val("");
        }
    }

    return conn;
}


function setClipboard(name) {
    var clipboard = new ClipboardJS(name);

    clipboard.on('success', function (e) {
        console.log(e);
    });

    clipboard.on('error', function (e) {
        console.log(e);
    });
}


function copy() {
    $("#copyText").select();
    var success = document.execCommand('copy', false, null);
    set("copyStatus", success);
}

function set(key, value) {
    var date = new Date();
    //客户端缓存时间  默认一天
    date.setDate(date.getDate() + 1);
    var expiredTime = date.getTime();
    localStorage.setItem(key, JSON.stringify({ data: value, time: expiredTime }));
}

function get(key) {
    var data = localStorage.getItem(key);
    if (!data) return null;
    var dataObj = JSON.parse(data);
    if (new Date().getTime() > dataObj.time) {
        return null;
    } else {
        var dataObjDatatoJson = JSON.parse(dataObj.data)
        return dataObjDatatoJson;
    }
}








//页面初始化
function init() {

    if (!get("copyStatus")) {
        $("body").one("click", copy);
    }
    //设置返回顶部按钮
    setGoTop();
    setLoading(10);

    //刷新页底菜单
    refreshFooterNav();
    setLoading(20);

    //滚动控制按钮显示
    $(window).scroll(function () {
        setGoTop();
    });
    //返回顶部按钮绑定事件
    $('#gotop').click(goTop);
    setLoading(50);

}

//页面资源加载完成
document.onreadystatechange = function () {
    if (document.readyState == 'complete') {
        setLoading(100);
    }
};


$(init);