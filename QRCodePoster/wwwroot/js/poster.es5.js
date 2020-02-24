﻿'use strict';

$('form').submit(function () {
	var data = [];
	$('.drag').each(function () {
		var obj = $(this);
		var type = obj.attr('type');
		var left = obj.css('left'),
		    top = obj.css('top');
		var d = { left: left, top: top, type: obj.attr('type'), width: obj.css('width'), height: obj.css('height') };
		if (type == 'nickname' || type == 'title' || type == 'marketprice' || type == 'productprice') {
			d.size = obj.attr('size');
			d.color = obj.attr('color');
		} else if (type == 'qr') {
			d.size = obj.attr('size');
		} else if (type == 'img') {
			d.src = obj.attr('src');
		}
		data.push(d);
	});
	$(':input[name=Data]').val(JSON.stringify(data));

	return true;
});

function bindEvents(obj) {
	var index = obj.attr('index');

	var rs = new Resize(obj, { Max: true, mxContainer: "#poster" });
	rs.Set($(".rRightDown", obj), "right-down");
	rs.Set($(".rLeftDown", obj), "left-down");
	rs.Set($(".rRightUp", obj), "right-up");
	rs.Set($(".rLeftUp", obj), "left-up");
	rs.Set($(".rRight", obj), "right");
	rs.Set($(".rLeft", obj), "left");
	rs.Set($(".rUp", obj), "up");
	rs.Set($(".rDown", obj), "down");
	rs.Scale = true;
	var type = obj.attr('type');
	if (type == 'nickname' || type == 'img' || type == 'title' || type == 'marketprice' || type == 'productprice' || type == 'thumb') {
		rs.Scale = false;
	}
	new Drag(obj, { Limit: true, mxContainer: "#poster" });
	$('.drag .remove').unbind('click').click(function () {
		$(this).parent().remove();
	});

	$.contextMenu({
		selector: '.drag[index=' + index + ']',
		callback: function callback(key, options) {
			var index = parseInt($(this).attr('zindex'));

			if (key == 'next') {
				var nextdiv = $(this).next('.drag');
				if (nextdiv.length > 0) {
					nextdiv.insertBefore($(this));
				}
			} else if (key == 'prev') {
				var prevdiv = $(this).prev('.drag');
				if (prevdiv.length > 0) {
					$(this).insertBefore(prevdiv);
				}
			} else if (key == 'last') {
				var len = $('.drag').length;
				if (index >= len - 1) {
					return;
				}
				var last = $('#poster .drag:last');
				if (last.length > 0) {
					$(this).insertAfter(last);
				}
			} else if (key == 'first') {
				var index = $(this).index();
				if (index <= 1) {
					return;
				}
				var first = $('#poster .drag:first');
				if (first.length > 0) {
					$(this).insertBefore(first);
				}
			} else if (key == 'delete') {
				$(this).remove();
			}
			var n = 1;
			$('.drag').each(function () {
				$(this).css("z-index", n);
				n++;
			});
		},
		items: {
			"next": { name: "调整到上层" },
			"prev": { name: "调整到下层" },
			"last": { name: "调整到最顶层" },
			"first": { name: "调整到最低层" },
			"delete": { name: "删除元素" }
		}
	});
	obj.unbind('click').click(function () {
		bind($(this));
	});
}
var imgsettimer = 0;
var nametimer = 0;
var bgtimer = 0;
function bindType(type) {
	$("#goodsparams").hide();
	$(".type4").hide();
	if (type == '4') {
		$(".type4").show();
	} else if (type == '商品') {
		$("#goodsparams").show();
	}
}
function clearTimers() {
	clearInterval(imgsettimer);
	clearInterval(nametimer);
	clearInterval(bgtimer);
}

function getImgUrl(val) {

	if (val.indexOf('http://') == -1) {
		val = "/attachment/" + val;
	}
	return val;
}
function bind(obj) {
	var imgset = $('#imgset'),
	    nameset = $("#nameset"),
	    qrset = $('#qrset');
	imgset.hide(), nameset.hide(), qrset.hide();
	clearTimers();
	var type = obj.attr('type');
	if (type == 'img' || type == 'thumb') {
		imgset.show();
		var src = obj.attr('src');
		var input = imgset.find('input');
		var img = imgset.find('img');
		if (typeof src != 'undefined' && src != '') {
			input.val(src);
			img.attr('src', getImgUrl(src));
		}

		imgsettimer = setInterval(function () {
			if (input.val() != src && input.val() != '') {
				var url = getImgUrl(input.val());

				obj.attr('src', input.val()).find('img').attr('src', url);
			}
		}, 10);
	} else if (type == 'nickname' || type == 'title' || type == 'marketprice' || type == 'productprice') {

		nameset.show();
		var color = obj.attr('color') || "#000";
		var size = obj.attr('size') || "16";
		var input = nameset.find('input:first');
		var namesize = nameset.find('#namesize');
		var picker = nameset.find('.sp-preview-inner');
		input.val(color);namesize.val(size.replace("px", ""));
		picker.css({ 'background-color': color, 'font-size': size });

		nametimer = setInterval(function () {
			obj.attr('color', input.val()).find('.text').css('color', input.val());
			obj.attr('size', namesize.val() + "px").find('.text').css('font-size', namesize.val() + "px");
		}, 10);
	} else if (type == 'qr') {
		qrset.show();
		var size = obj.attr('size') || "3";
		var sel = qrset.find('#qrsize');
		sel.val(size);
		sel.unbind('change').change(function () {
			obj.attr('size', sel.val());
		});
	}
}

$(function () {
	$(':radio[name=Type]').click(function () {
		var type = $(this).val();
		bindType(type);
	});
	//改变背景
	$('#bgset').find('button:first').click(function () {
		var oldbg = $(':input[name=BGUrl]').val();
		bgtimer = setInterval(function () {
			var bg = $(':input[name=BGUrl]').val();
			if (oldbg != bg) {
				bg = getImgUrl(bg);

				$('#poster .bg').remove();
				var bgh = $("<img src='" + bg + "' class='bg' />");
				var first = $('#poster .drag:first');
				if (first.length > 0) {
					bgh.insertBefore(first);
				} else {
					$('#poster').append(bgh);
				}

				oldbg = bg;
			}
		}, 10);
	});

	$('.btn-com').click(function () {
		var imgset = $('#imgset'),
		    nameset = $("#nameset"),
		    qrset = $('#qrset');
		imgset.hide(), nameset.hide(), qrset.hide();
		clearTimers();

		if ($('#poster img').length <= 0) {
			//alert('请选择背景图片!');
			//return;
		}
		var type = $(this).data('type');
		var img = "";
		if (type == 'qr') {
			img = '<img src="/images/qr.jpg" />';
		} else if (type == 'head') {
			img = '<img src="/images/head.jpg" />';
		} else if (type == 'img' || type == 'thumb') {
			img = '<img src="/images/img.jpg" />';
		} else if (type == 'nickname') {
			img = '<div class=text>昵称</div>';
		} else if (type == 'title') {
			img = '<div class=text>商品名称</div>';
		} else if (type == 'marketprice') {
			img = '<div class=text>商品现价</div>';
		} else if (type == 'productprice') {
			img = '<div class=text>商品原价</div>';
		}
		var index = $('#poster .drag').length + 1;
		var obj = $('<div class="drag" type="' + type + '" index="' + index + '" style="z-index:' + index + '">' + img + '<div class="rRightDown"> </div><div class="rLeftDown"> </div><div class="rRightUp"> </div><div class="rLeftUp"> </div><div class="rRight"> </div><div class="rLeft"> </div><div class="rUp"> </div><div class="rDown"></div></div>');

		$('#poster').append(obj);

		bindEvents(obj);
	});

	$('.drag').click(function () {
		bindEvents($(this));
	});
});

