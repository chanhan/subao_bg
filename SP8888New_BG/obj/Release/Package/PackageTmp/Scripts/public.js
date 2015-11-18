// 给日期类对象添加日期差方法，返回日期与参数日期的时间差，单位为天
Date.prototype.Subtract = function (date) {
    return (this.getTime() - date.getTime()) / (24 * 60 * 60 * 1000);
}

// 给日期类对象添加返回格式化日期的处理方法，日期可加减天数
Date.prototype.GetFormateAddDate = function (value, join) {
    if (join == undefined) join = '-';
    if (value != undefined && !isNaN(value)) {//传入加减天数的参数，且为数字
        this.setDate(this.getDate() + value);
    }
    var month = this.getMonth() + 1;
    month = month < 10 ? "0" + month : month;
    var dateStr = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
    return this.getFullYear() + join + month + join + dateStr;
}
Date.prototype.addDay = function (num) {
    this.setDate(this.getDate() + num);
    return this;
}

Date.prototype.addMonth = function (num) {
    var tempDate = this.getDate();
    this.setMonth(this.getMonth() + num);
    if (tempDate != this.getDate()) this.setDate(0);
    return this;
}

Date.prototype.addYear = function (num) {
    var tempDate = this.getDate();
    this.setYear(this.getYear() + num);
    if (tempDate != this.getDate()) this.setDate(0);
    return this;
}
//传入日期年月日，返回获取日期
function GetDate(date) {
    return new Date(date[0], date[1] - 1, date[2]);
}
//预览上传的图片
function PreviewImage(id, input, imageExtension) {
    if (input.files && input.files[0] && (typeof (imageExtension) == 'undefined' ? /\.(gif|jpg|jpeg|png|GIF|JPG|JPEG|PNG)$/.test(input.files[0].name) : input.files[0].name.substring(input.files[0].name.indexOf('.') + 1).toUpperCase() == imageExtension.toUpperCase())) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + id).removeAttr('src').attr('src', e.target.result).css('display', 'block');
        }
        reader.readAsDataURL(input.files[0]);
    } else {
        input.value = '';
        $('#' + id).attr('src', '');
        alert(typeof (imageExtension) == 'undefined' ? '圖片必須是.gif,jpeg,jpg,png中的一種' : '圖片必須是.' + imageExtension + '格式');
    }
}
//验证url合法性
function MatchUrl(url) {
    url = $.trim(url);
    return url == '' ? true : /^((http|https|ftp):\/\/)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(\/[a-zA-Z0-9\&%_\./-~-]*)?$/.test(url);
}
$(function () {
    SetWindowHeight();
    SetliHeight();
    $(window).resize(SetWindowHeight);
})
//设置窗口宽度
function SetWindowHeight() {
    var screenWidth = window.screen.width;
    var winHeight;
    //获取窗口高度
    if (window.innerHeight)
        winHeight = window.innerHeight;
    else if ((document.body) && (document.body.clientHeight))
        winHeight = document.body.clientHeight;
    $("#warpper").width(screenWidth);
    $("#warpper").height(winHeight);
    $("#topR").css("padding-left", $("#right").width()-50+"px");
}
//设置li高度
function SetliHeight() {
    var height = 0;
    var index = [];
    var count = 0;
    var minHeight = 30;
    $('dd:gt(0) ul').each(function () {
        if (typeof ($(this).attr('auotoheight')) != 'undefined') return;
        $(this).children('li').each(function () {
            var h = $(this).height();
            if (h > height) {
                height = h;
                index = [];
                index.push(count);
            } else if (h == height) {
                index.push(count);
            }
            count++;
        });
        if (height < minHeight) {
            height = minHeight;
            $(this).children('li').height(height).css("line-height", height + "px");
        } else {
            $(this).children('li').height(height);
            count = 0;
            $(this).children('li').each(function () {
                if (typeof ($(this).attr('auotoheight')) == 'undefined' && index.indexOf(count) < 0) {
                    $(this).css("line-height", height + "px");
                }
                count++;
            })
        }
        height = 0;
        count = 0;
        index = [];
    })
}
//重设序号
function ResetIndex() {
    var index = 1;
    $('dd:visible:gt(0)').each(function () {
        $(this).find('li:eq(0)').html(index);
        SetColor($(this).find('li'), index);
        index++;
    })
}
//重设背景色
function ResetBackGroundColor() {
    var index = 1;
    $('.tabdl2 dd:visible:gt(0)').each(function () {
        SetColor($(this).find('li'), index);
        index++;
    })
}
function SetColor(obj, index) {
    if (index % 2 == 1) obj.removeClass('odd').addClass('even');
    else obj.removeClass('even').addClass('odd');
}

//快捷键（CTRL+N)的功能--开始
function NewWindow() {
    window.open(window.location.href, "_blank");
}
document.onkeydown = function (e) {
    var e = e || event;
    if (e.ctrlKey) {
        var keyNum = e.which || e.keyCode;
        if (keyNum == 78) {
            NewWindow();
        }
    }
}
//添加快捷键（CTRL+N)的功能--结束