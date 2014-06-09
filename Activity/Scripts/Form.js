
function HammerAjax(url, data, fn, async) {
    if(async == null)
    {
        async = true;
    }
	$.ajax({
		type: "post",
		url: url,
		dataType: "json",
		data: data,
        async: async,
		success: fn
	});
}

function getFormJson(frm) {
	var o = {};
	var a = $(frm).serializeArray();
	$.each(a, function () {
		if (o[this.name] != undefined) {
			if (!o[this.name].push) {
				o[this.name] = [o[this.name]];
			}
			o[this.name].push(this.value || '');
		} else {
			o[this.name] = this.value || '';
		}
	});

	return o;
}

$.fn.GetPostData = function () {
    var data = {};

    $(this).find("[col]").each(function (i, value) {

        var field = $(value).attr("col");

        if (value.tagName == "INPUT") {
            if (value.type == "checkbox") {
                if ($(value).attr("checked") == true) {
                    if (data[field]) {
                        data[field] = data[field] + "," + $(value).val();
                    } else {
                        data[field] = $(value).val();
                    }
                }
            }
            else if (value.type == "radio") {
                if ($(value).attr("checked") == true) {
                    data[field] = $(value).val();
                }
            }
            else {
                data[field] = $(value).val();
            }
        }

        else if (value.tagName == "SELECT") {
            data[field] = $(value).val();
        }
        else if (value.tagName == "DIV") {
            data[field] = $(value).html();
        }
        else if (value.tagName == "IMG") {
            data[field] = $(value).attr("src");
        }
        else if (value.tagName == "SPAN") {
            data[field] = $(value).html();
        }
        else if (value.tagName == "TEXTAREA") {
            data[field] = $(value).val();
        }

    });
    return data;
}