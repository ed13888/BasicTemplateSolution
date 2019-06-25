//全局公用函数

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



function set(key, value, days) {
    days = days || 1;
    var date = new Date();
    //客户端缓存时间  默认一天
    date.setDate(date.getDate() + days);
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


//产生随机数
function rondom(min, max) {
    //parseInt(Math.random() * (max - min + 1) + min, 10);
    return Math.floor(Math.random() * (max - min + 1) + min);
}

//音乐播放器
function AudioPlayer(audio, switchButton) {

    var _this = this, _audioPlayer = audio, _switchButton = switchButton;

    //播放
    this.play = function (url) {
        if (url) _audioPlayer.setAttribute("src", url);
        _audioPlayer.play();
        return _this;
    };

    //暂停
    this.pause = function () {
        _audioPlayer.pause();
        return _this;
    };

    //播放\暂停
    this.toggle = function () {
        if (_audioPlayer.paused) {
            _audioPlayer.play();
        } else {
            _audioPlayer.pause();
        }
        return _this;
    };

    //音乐开关按钮
    if (_switchButton) {
        if (!_switchButton.hasClass("music")) {
            _switchButton.addClass("music");
        }
        //默认开启状态
        _switchButton.addClass("music-open");
        _switchButton.click(function () {
            _this.toggle();
            $(this).toggleClass("music-open").toggleClass("music-close");
        });
    }

}



