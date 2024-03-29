﻿//站点逻辑函数  防止全局污染
(function ($) {

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



    //页面初始化
    function init() {

        //if (!get("copyStatus")) {
        //    $("body").one("click", copy);
        //}
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

        //请求字体文件
        $.ajax({
            type: 'GET',
            url: '/fonts/zh7.ttf',
            dataType: 'text',
            ifModified: true,
            cache: true,
            success: function () {
                $("body").addClass("body-font");
            },
        });

        setLoading(50);

    }

    //页面资源加载完成
    document.onreadystatechange = function () {
        if (document.readyState == 'complete') {
            setLoading(100);
            //$("body").addClass("body-font");
        }
    };

    //初始化
    $(init);


})(jQuery);



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

function starNum(id, value, _complete) {
    var temp = $("#" + id);
    complete = function () {
        _complete && _complete(temp);
    };
    temp.animate({ count: value }, {
        duration: 1000,
        step: function () {
            temp.text(parseInt(this.count));
        },
        complete: complete
    });
}

//banner动画
function animateImgBlock() {

    var imglist = $(".img-block");
    var len = imglist.length;
    var ran = rondom(0, len - 1);
    var animateRandom = rondom(0, $.animateList.length - 1);
    var animateDom = $(imglist[ran]);
    var animate = $.animateList[animateRandom];
    var delay = "delay-1s";

    setTimeout(function () {
        animateDom.addClass(animate.in);
    }, 1000);


    setTimeout(function () {
        animateDom.removeClass(animate.in);
        animateImgBlock();
    }, 2000);
}