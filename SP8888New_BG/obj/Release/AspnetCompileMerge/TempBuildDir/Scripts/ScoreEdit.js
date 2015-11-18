// setTimeout id
var timeout = -1;

//StatusText 单选显示隐藏
function notshow(obj, item) {
    //選取時將隱藏
    document.getElementById(item).style.display = obj.checked ? "none" : "inline";
}
function show(obj, item) {
    if ($(obj).is(":checked")) {
        $("#" + item).show();
    }
}

$(document).ready(function () {
    //隱藏時間欄位
    $("#spanTime").hide();
    //頁面初始化
    onInit();

    // 2015/05/15, 輸入驗證
    onInputNumberCheck();

    //2015/05/14, 處理比賽局數選擇鈕的事件
    onRunListChanged();
    //2015/05/14, input keyup 事件
    onInputKeyup();

    //解決ie9下 disabled之後可以選中
    $("input[type='text']:disabled").on("mouseup", function (evt) {
        if ($(this).attr("disabled") == true) {
            this.blur();
        }
    });
});

// 設定加減分
function setPoints($input, num, plus) {

    // 加分 or 減分
    num = (plus) ? num : num * -1;

    var v = $input.val();
    var score = num;
    if ($.isNumeric(v)) { score += parseInt(v, 10); }
    // 設定 H 分
    if (score < 0) { score = 0; }
    $input.val(score);
}

// 儲存加減分數
function savePoints(Team, Number, Rhe, plus) {

    var $divRun = $(Team);
    var $inputRun = $divRun.find("input[type='text']");
    var $inputRHE = $(Rhe).find("input[type='text']");
    // 找尋有點選局數按鈕
    var $radio = $("input[type='radio'][name='runlit']:checked");
    // 未點選按鈕則不處理
    if ($radio.length === 0) { return; }

    // 局數
    var $input = null;
    var inning = $radio.val();
    if (inning === "HRun" || inning === 'ERun') {
        // 點選 H 分或 E 分
        $input = $inputRHE.filter("[c='" + inning + "']");
        setPoints($input, Number, plus);
    } else {
        // 點選賽局比分
        $input = $inputRun.filter("[c='" + inning + "']");
        setPoints($input, Number, plus);
        // 計算總分
        var $inputTotal = $inputRHE.filter("[c='RunTotal']");
        sumTotalScore($divRun, $inputTotal);
    }
    saveScore(false);
}

//加分
function Ppoints(Team, Number, Rhe) {
    savePoints(Team, Number, Rhe, true);
}

//減分
function Mpoints(Team, Number, Rhe) {
    savePoints(Team, Number, Rhe, false);
}

//計時
function BtnCount(IsPostBack) {
    $("#Clicked").hide();
    $("#Closed").show();
    var count = $("#minutes").val();
    var count2 = $("#seconds").val();

    if ($.isNumeric(count) === false) { count = 0; }
    if ($.isNumeric(count2) === false) { count2 = 0; }

    //驗證輸入資料
    if (count > 60) {
        alert("不可大於60");
        return;
    }
    else if (count2 > 59) {
        alert("不可大於59");
        return;
    }
    //關閉編輯
    $("#minutes").attr("readonly", true);
    $("#seconds").attr("readonly", true);

    //檢核輸入長度
    if (count.length == 1) {
        count = checkTime(count);
        $("#minutes").val(count.toString());
    }

    //"啟動"，則+1
    if (IsPostBack == 1) {
        count2 = parseInt(count2) + 1;
        if (count2 != 0) {
            count2--;
            count2 = checkTime(count2);
            $('#seconds').val(count2.toString());
            timeout = setTimeout(BtnCount, 1000);
        }
        else {
            count--;
            count = checkTime(count);
            $('#minutes').val(count.toString());
            $('#seconds').val(parseInt(count2) + 59);
            timeout = setTimeout(BtnCount, 1000);
        }
    }
        //否則則開始計算
    else {
        if (count2 != 0) {
            count2--;
            count2 = checkTime(count2);
            $('#seconds').val(count2.toString());
            timeout = setTimeout(BtnCount, 1000);
        }
            //時間皆為0則停止時間
        else if (count == 0 && count2 == 0) {
            clearTimeout(timeout);
            $("#Closed").hide();
            $("#Clicked").show();

            $("#minutes").prop("disabled", false);
            $("#seconds").prop("disabled", false);

            saveScore(false);
        }
        else {
            count--;
            count = checkTime(count);
            $('#minutes').val(count.toString());
            $('#seconds').val(parseInt(count2) + 59);
            timeout = setTimeout(BtnCount, 1000);
        }
    }

}

//停止時間
function outTime() {
    $("#Closed").hide();
    $("#Clicked").show();
    clearTimeout(timeout);
    $("#minutes").prop("readonly", false);
    $("#seconds").prop("readonly", false);
    //送出時間和比分
    saveScore(false);
}

//檢查是否小於0，自動補零
function checkTime(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}

// 2014/04/14, 重新計算總分
function sumTotalScore($div, $inputTotal) {

    var total = 0;
    $div.find("input[type='text']").each(function () {
        // 取得分數(球賽可輸入X, 表示賽局結束)
        var val = $(this).val().toUpperCase().replace(/\X/g, "");
        if ($.isNumeric(val)) {
            total += parseInt(val, 10);
        }
    });

    $inputTotal.val(total);
}

// 2015/05/12 選取上面單選事件, 
function onRunListChanged() {
    $("#liTitle").on("click", "input[type='radio'][name='runlit']", function () {
        // 清除剩餘時間狀態
        clearStatus();
        //關閉所有輸入方塊
        var $div = $("div[id*='run'], #rheA, #rheB");
        $div.find("input:not([readonly]):enabled").prop("disabled", true);
        var $this = $(this);

        //開啟點選該局的輸入方塊
        if ($this.prop("checked") === true) {
            var val = $this.val();
            $div.find("input[c='" + val + "']").prop("disabled", false);
        }
    });
}

//2014/04/14, input keyup 事件
function onInputKeyup() {
    //$("div[id*='run']").on("keyup", "input.open_input", function () {
    $("div[id*='run']").on("keyup", "input[type='text']:enabled", function () {
        var $div = $(this).closest("div");
        var $inputTotal = $div.siblings("div").find("input[c='RunTotal']");
        // 重新計算加總
        sumTotalScore($div, $inputTotal);
        $("#message").html("尚未儲存。");
        $("#message2").hide();
    });
}

// 2015/05/14, 輸入類型鎖定
function onInputNumberCheck() {
    $("div[id*='run']").on("keydown", "input[type='text']:enabled", function (e) {
        var key = e.which ? e.which : e.keyCode;
        // Allow: backspace, delete, tab and escape
        if (key === 46 || key === 8 || key === 9 || key === 27) {
            return;
        } else {
            // 除數字以外
            if (e.shiftKey || (key < 48 || key > 57) && (key < 96 || key > 105)) {
                // 允許輸入 X, 表示賽局結束
                if (key === 88 || key === 120) { return; }
                // 阻擋預設要發生的事件
                if (document.all) {
                    window.event.returnValue = false;
                } else {
                    e.preventDefault();
                }
            }
        }
    });
}

// 2014/04/17, 頁面初始化
function onInit() {
    //清除訊息
    $("#message").html("");
    var $radio = $("input[type='radio'][name='runlit']");
    var $div = $("div[id*='runs']");

    // 取得分數有值項目
    var item = [];
    var $input = $div.find("input[type='text']");
    $input.each(function () {
        var $this = $(this);
        if ($this.val() !== '') {
            var c = $this.attr("c");
            if ($.inArray(c, item) === -1) { item.push(c); }
        }
    });

    // 都沒輸入時, 預設開啟第一個; 否則檢查是否開啟下一個
    if (item.length === 0) {
        item.push('1');
    } else {
        var k = item[item.length - 1];
        // 檢查最後一局是否都有 key 值
        var $score = $input.filter("[c='" + k + "']").map(function () {
            var v = $(this).val();
            if (v !== '') { return v; }
        });

        // 都有值才開啟下一個
        if ($score.length === 2) {

            //判斷是不是最後一局, 是的話自動新增 OT 賽局
            var isEndGame = judgeEndGame($radio, k);
            if (isEndGame) {
                addOT($radio, $div, k);
            }

            var next = parseInt(k, 10) + 1;
            item.push(next.toString());
        }
    }

    // 重新取得選取按鈕
    $radio = $($radio.selector);
    // 開啟在清單內的選取按鈕
    $radio.each(function () {
        var $this = $(this);
        if ($.inArray($this.val(), item) >= 0) {
            $this.prop("disabled", false);
        }
    });

    // 清除剩餘時間狀態
    clearStatus();

    // 動態設定大小
    dynamicSetTableWidth($radio);
}

//動態設定大小
function dynamicSetTableWidth() {
    var len = $("#runsA input,#rheA input").length;
    // Team(158) + 局數各數(55) + 右侧预留 - 以上數值預留 padding 
    var newWidth = 140 + len * 55 + 78;
    var $table = $(".tabdl-score dl dd:first-child ul li");
    if ($table.outerWidth(true) <= newWidth) {
        $table.outerWidth(newWidth);
        $(".tabdl-score dl dd").not(":first-child").find("ul li").not(":first-child").outerWidth(newWidth - $(".tabdl-score dl dd").not(":first-child").find("ul li:first-child").outerWidth(true));
        $(".tabdl-score dl dd").slice(4).find("ul li").outerWidth(newWidth);
        $(".sc-tabbox").width(newWidth);
    }

}

// 清除剩餘時間狀態
function clearStatus() {
    // 取得剩餘時間的所有 input
    var $input = $("#pTime input");
    // 判斷是否有勾選倒數
    var $btnClosed = $("#Closeed");
    var $btnClicked = $("#Clicked");
    //var checked = $("#MainContent_rabDownTime").prop("checked");
    // 若已開始倒數
    if ($btnClosed.is(":visible")) {

        $btnClosed.hide();
        $btnClicked.show();
        // 清除倒數
        clearTimeout(timeout);
    }

    // 清除所有文字方塊
    $input.filter("[type='text']").val("").prop("readonly", false);
    // 隱藏時間區塊
    $("#spanTime").hide();
    // 取消所有選取
    $input.filter("[type='radio']").prop("checked", false);

    //取消 結束 中場休息 
    $('input[name=gamestate]:checked').prop("checked", false);
}

// 判斷是否是最後一局
function judgeEndGame($radio, inning) {
    return $radio.filter("[t='jushu']").last().val() == inning;
}

// 自動增加 OT 賽局
function addOT($radio, $div, inning) {
    // 欲新增局數
    var k = parseInt(inning, 10) + 1;
    //var ot = parseInt($("#trTitle td:last-child").text().split('OT')[1], 10) + 1;
    var gameType = $("#GameType").text();
    var $tdR = $("#spanR"); //在R前面插入

    // 查詢是否已經有同樣的 OT 賽局, 若有則不新增
    var len = $radio.filter("[value='" + k + "']").length;
    if (len >= 1) { return; }

    if (gameType.indexOf("BB") != 0) {
        addOTBaseBall($div, $tdR, k);
    } else if (gameType === "BK") {
        addOTBasketBall($div, $tdR, k);
    } else if (gameType === "IH") {
        // 預設全建立 ( 3局基本 + OT, SO 各一 )
        //addOTIH($div, $tdR, k);
    } else if (gameType === "AF") {
        addOTAF($div, $tdR, k);
    } else if (gameType === "UF") {
        // UF 僅一局 R, 無加時
    }
}

//2014/04/15, 儲存事件
function onSaveBtnClick() {

    $("#btnStore").on("click", function () {
        // 取得目前有開啟點選的局數
        var innings = $("input[type='radio'][name='runlit']:enabled").map(function () {
            var $this = $(this);
            var c = $this.attr("c");
            if (c !== 'HRun' && c !== 'ERun') {
                return $(this).val();
            }
        }).toArray();

        // TeamA 比分
        var scoreA = getInningScore($("#runA_div input[type='text']"), innings);
        var runsA = scoreA.join(",");
        // TeamB 比分
        var scoreB = getInningScore($("#runB_div input[type='text']"), innings);
        var runsB = scoreB.join(",");

        // GID
        var gid = $("#serial").text();
        // 賽別
        var gameType = $("#title").text();
        // RHEA, RHEB比分 
        var rheA = getRHEScore($("#rheA_div input[type='text']"));
        var rheB = getRHEScore($("#rheB_div input[type='text']"));

        // 判斷剩餘比賽狀態
        var $downTime = $("#MainContent_rabDownTime");
        var $end = $("#MainContent_rabEnd");
        var $intermission = $("#MainContent_rabIntermission");
        var statusText = "";

        if ($downTime.prop("checked") === true) {
            // 點選倒數
            statusText = getCountDownTime();
        } else if ($end.prop("checked") === true) {
            // 點選結束
            statusText = $end.val();
            if (window.confirm("確定是否選擇結束選項？")) {
                actionForIntermissionOrEnd();
            }
            else {
                return;
            }
        } else if ($intermission.prop("checked") === true) {
            // 點選結束
            statusText = $intermission.val();
            if (window.confirm("確定是否選擇中場休息選項？")) {
                actionForIntermissionOrEnd();
            }
            else {
                return;
            }
        }

        var data = null;
        if (gameType === 'BB') {
            data = { "gid": gid, "title": gameType, "runA": runsA, "runB": runsB, "RA": rheA[0], "RB": rheB[0], "HA": rheA[1], "HB": rheB[1], "EA": rheA[2], "EB": rheB[2], "StatusText": statusText };
        } else {
            data = { "gid": gid, "title": gameType, "runA": runsA, "runB": runsB, "RA": rheA[0], "RB": rheB[0], "StatusText": statusText };
        }

        var $msg = $("#message");
        $.post("Scoreedit.ashx", data).done(function () {
            // 頁面初始化
            onInit();
            //$msg.text("儲存成功。");
        }).fail(function () {
            //alert("儲存失敗");
            //window.location.reload();
            $msg.html("儲存失敗。");
        });

    });
}

// 取得局數比分
function getInningScore($input, innings) {
    var $item = $input.map(function () {
        var $this = $(this);
        // 判斷是否在局數之內
        if ($.inArray($this.attr("c"), innings) >= 0) {
            var v = $this.val();
            // 若是最後一局且沒有值, 不儲存
            if (!($this.is(":last-child") && v === '')) {
                return v;
            }
        }
    });
    return $item.toArray();
}

// 取得 RHE 比分
function getRHEScore($input) {
    var $item = $input.map(function () {
        var v = $(this).val();
        return (v === '') ? '0' : v;
    });
    return $item.toArray();
}

// 取得倒數時間文字
function getCountDownTime(m, s) {
    var min = $(m).val();
    var second = $(s).val();
    var text = "";

    if (min.length === 1 && second.length === 1) {
        text = "0" + min + ":0" + second;
    } else if (min.length === 1 && second.length !== 1) {
        text = "0" + min + ":" + second;
    } else if (min.length !== 1 && second.length === 1) {
        text = min + ":0" + second;
    } else {
        text = min + ":" + second;
    }

    return text;
}

// 儲存時,中場休息/結束處理
function actionForIntermissionOrEnd() {
    // 清除 timeout
    clearTimeout(timeout);
    $("#Closeed").hide();
    $("#Clicked").show();

    $("#MainContent_ctrText").prop("readonly", false);
    $("#MainContent_ctrText2").prop("readonly", false);
}

//中場休息/結束處理 確認框
function StatusTextConfirm(v) {
    if (window.confirm("確定是否選擇[" + (v == "" ? "清除所有狀態" : v) + "]選項？")) {
        return true;
    }
    return false;
}

// 新增棒球 OT 賽局
function addOTBaseBall($div, $tdR, inning) {
    // 選取按鈕    
    $tdR.before("<span class=\"radiospan-score\"><input type=\"radio\"  value=\"" + inning + "\" name=\"runlit\" disabled='disabled' t=\"jushu\"><label>" + inning + "</label></span>");
    $div.each(function () {
        var wz = this.id.indexOf("A") != -1 ? "A" : "B";
        // 文字方塊    
        $(this).append("<input name=\"txtRuns" + wz + "_" + inning + "\" disabled=\"disabled\" id=\"txtRuns" + wz + "_" + inning + "\" type=\"text\" c=\"" + inning + "\" value=\"\"/>");
    });

}

// 新增籃球 OT 賽局
function addOTBasketBall($div, $tdR, inning) {

    var alliance = $("#Alliance").text();
    // 基本局數 - NCAA: 2; 其他: 4 (計算OT局數用) 
    var basicInnings = (alliance === 'BKNCAA') ? 2 : 4;

    // 選取按鈕
    $tdR.before("<td class='scoreWidth'>OT" + (inning - basicInnings) + "<input type='radio' name='runlit' value='" + inning + "' disabled='disabled' ></td>");
    // 文字方塊
    $div.append("<input type='text' class='scoreWidth' value='' c='" + inning + "' class='child" + inning + "' disabled='disabled' />")
}

// 新增冰球 OT 賽局
function addOTIH($div, $tdR, inning) {

    var text = inning;
    if (text === 4) {
        text = "OT";
    } else if (text === 5) {
        text = "SO";
    }

    // 選取按鈕
    $tdR.before("<td class='scoreWidth'>" + text + "<input type='radio' name='runlit' value='" + inning + "' disabled='disabled' ></td>");
    // 文字方塊
    $div.append("<input type='text' class='scoreWidth' value='' c='" + inning + "' class='child" + inning + "' disabled='disabled' />")
}


// 新增美足 AF 賽局
function addOTAF($div, $tdR, inning) {

    var basicInnings = 4;
    // 選取按鈕
    $tdR.before("<td class='scoreWidth'>OT" + (inning - basicInnings) + "<input type='radio' name='runlit' value='" + inning + "' disabled='disabled' ></td>");
    // 文字方塊
    $div.append("<input type='text' class='scoreWidth' value='' c='" + inning + "' class='child" + inning + "' disabled='disabled' />")
}