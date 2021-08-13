/*globals jQuery, define, module, exports, require, window, document, postMessage, config */
(function (factory) {
	"use strict";
	if (typeof define === 'function' && define.amd) {
		define(['jquery', 'plupload/base', 'jquery.serialize'], factory);
	}
	else if(typeof module !== 'undefined' && module.exports) {
		module.exports = factory(require('jquery'), require('plupload/base'), require('jquery.serialize'));
	}
	else {
		factory(jQuery, plupload, jQuery.serialize);
	}
}(function ($, plupload, serialize, undefined) {
	"use strict";

	if($.parse) { return; }

	$.parse = {};
	$.parse.defaults = {
		runtimes     : 'html5,html4',
		url          : typeof config === 'object' && config.upload ? config.upload : '',
		chunksize    : typeof config === 'object' && config.chunksize ? config.chunksize : '1mb',
		multipart    : {},
		disabled     : false,
		multiple     : false,
		images       : false,
		download     : false,
		settings     : false,
		browse       : {
			clss : 'btn btn-info',
			html : 'Прикачи',
			icon : 'fa fa-plus'
		},
		zip          : {
			clss : 'btn btn-info',
			html : 'Изтегли',
			icon : 'fa fa-archive'
		},
		remove       : {
			clss : '',
			html : '',
			icon : 'fa fa-remove'
		},
		file         : {
			done : 'fa fa-file',
			wait : 'fa fa-refresh',
			settings : 'fa fa-cog',
			drag : 'fa fa-arrows-v'
		},
		value        : null,
		dnd          : true,
		changed      : null,
		edited       : null,
	};
	if(typeof config === 'object' && config.token) {
		$.parse.defaults.multipart = { "_csrf_token" : config.token };
	}

	$.parse.create = function (el, settings) {
		if(!settings) { settings = {}; }
		var options = $.extend(true, {}, $.plupload.defaults, $.parse.defaults, settings, ($(el).data('parse') || {}));
		var params = options.multipart || {};
		if(typeof config === 'object' && config.token) {
			params["_csrf_token"] = config.token;
		}
		var uploader = new plupload.Uploader({
				runtimes			: options.runtimes,
				browse_button		: $(el)[0],
				multipart			: true,
				multipart_params	: params,
				url					: options.url,
				chunk_size			: options.chunksize,
				filters : [
					{
						title : (options.types ? "Files" : (options.images ? "Image files" : "All files")),
						extensions : (options.types ? options.types : (options.images ? "jpg,gif,png" : "*"))
					}
				]
			});
		uploader.init();
		return uploader;
	};
	$.fn.parse = function (settings) {
		return this.each(function () {
				var options     = $.extend(true, {}, $.parse.defaults, settings, ($(this).data('parse') || {})),
					field       = $(this),
					value       = $(this).val(),
					container   = null,
					upload      = null, i, j, temp,
					update      = function () {
						var tmp = [],
							files = [];
						container.find('.parse-file').each(function () {
							var id = $(this).data('id');//.toString();
							if(id) {
								id = id.toString();
								tmp.push(id);
								files.push({
									'id'    : id,
									'url'   : $(this).find('.parse-title').attr('href'),
									'hash'  : $(this).data('hash'),
									'html'  : $(this).find('.parse-title > span').text(),
									'thumb' : options.images ? $(this).css('backgroundImage').replace(/^url\(/i, '').replace(/\)$/, '') : ''
								});
							}
						});
						container.find('.parse-preview')
							.find(':checkbox').each(function () {
								tmp.push('c:' + (this.checked ? '1' : '0'));
							}).end()
							.find('select').each(function () {
								tmp.push('s:' + this.value);
							}).end();
						field.val(tmp.join(','));
						if(options.changed) {
							options.changed.call(this, options.multiple ? tmp : (tmp[0] || null), options.multiple ? files : (files[0] || null));
						}
					};

			// create container
			container = $(this).parent().find('[type="file"]').hide().end().end()
				.wrap('<div class="parse-container parse-multiple ' + (options.images ? 'parse-image-container' : 'parse-document-container') + (options.disabled ? ' parse-disabled' : '') + '"></div>').parent()
				.append('<div class="parse-buttons"></div><div class="parse-list"></div>')
				.children('.parse-buttons')
					.append('<a href="#" class="parse-browse '+options.browse.clss+'"><i class="'+options.browse.icon+'"></i> '+options.browse.html+'</a>').end();
			container.on('change', ':checkbox, select', function () { update(); });
			if(options.multiple && options.download) {
				container.children('.parse-buttons')
					.append('<a href="' + options.download + '" class="parse-zip '+options.zip.clss+'"><i class="'+options.zip.icon+'"></i> '+options.zip.html+'</a>');
			}
			if(options.value) {
				if(!$.isArray(options.value)) {
					options.value = [options.value];
				}
				for(i = 0, j = options.value.length; i < j; i++) {
					temp = $('' +
						'<div data-id="'+(options.value[i].id || '')+'" data-hash="'+(options.value[i].hash || '')+'" '+(options.images? 'style="background-image:url(\''+(options.value[i].thumb||options.value[i].url||'')+'\')"' : '')+' class="parse-complete parse-file ' + (!options.disabled && options.multiple && options.dnd ? ' parse-draggable ' : '') + (options.images ? 'parse-image' : '') + ' '+(options.value[i].clss||'')+'">' +
							(!options.disabled && options.multiple && options.dnd ? '<span class="parse-drag"><i class="'+(options.file.drag)+'"></i></span>' : '' ) +
							'<span class="parse-remove '+(options.remove.clss)+'"><i class="'+(options.remove.icon)+'"></i>'+options.remove.html+'</span>' +
							(options.settings ? '<span class="parse-settings"><i class="'+(options.file.settings)+'"></i></span>' : '') +
							'<small class="parse-details">'+(options.value[i].extra ? options.value[i].extra.join('&nbsp;&bull;&nbsp;') : '')+'</small>' +
							'<span class="parse-title" draggable="false"><i class="'+(options.value[i].icon||options.file.done)+'"></i><span>'+(options.value[i].settings && options.value[i].settings.name ? options.value[i].settings.name : options.value[i].html) +'</span></span>' +
							'<span class="parse-progress"><span class="parse-progress-inner" style="width:100%;"></span></span>' +
						'</div>'
					);
					container.children('.parse-list').append(temp);
					temp.data('settings', options.value[i].settings);
				}
			}
			if(!options.disabled && options.multiple && options.dnd) {
				var isdrg = 0,
					initx = false,
					inity = false,
					ofstx = false,
					ofsty = false,
					holdr = false,
					elmnt = false;
				container
					.on('mousedown', '.parse-title', function (e) {
						e.preventDefault();
						e.stopImmediatePropagation();
						return false;
					})
					.on('mousedown', '.parse-drag, .parse-image', function (e) {
						elmnt = $(this);
						if(elmnt.closest('.parse-disabled').length) {
							elmnt = false;
							return;
						}
						try {
							e.currentTarget.unselectable = "on";
							e.currentTarget.onselectstart = function() { return false; };
							if(e.currentTarget.style) { e.currentTarget.style.MozUserSelect = "none"; }
						} catch(err) { }
						holdr = false;
						initx = e.pageX;
						inity = e.pageY;
						elmnt = $(this).closest('.parse-file');
						var o = elmnt.offset();
						ofstx = e.pageX - o.left;
						ofsty = e.pageY - o.top;
						isdrg = 1;
					});
				$('body')
					.on('mousemove', function (e) {
						switch(isdrg) {
							case 0:
								return;
							case 1:
								if(Math.abs(e.pageX - initx) > 5 || Math.abs(e.pageY - inity)) {
									holdr = $('<div id="parse-holder" class="parse-file ' + (container.hasClass('parse-image-container') ? 'parse-image' : '') + ' parse-complete" style="width:' + elmnt.outerWidth() + 'px; height:' + elmnt.outerHeight() + 'px;"> </div>');
									elmnt.after(holdr);
									//elmnt.appendTo('body').css({ 'position' : 'absolute', 'left' : e.pageX + 4, 'top' : e.pageY + 8, 'zIndex' : 4 });
									elmnt.addClass('parse-dragged').parent().addClass('parse-dragging').end().appendTo('body').css({ 'position' : 'absolute', 'left' : e.pageX - ofstx, 'top' : e.pageY - ofsty });
									if(!container.hasClass('parse-image-container')) { elmnt.css('width', container.width()); }
									isdrg = 2;
								}
								break;
							case 2:
								elmnt.css({ 'left' : e.pageX - ofstx, 'top' : e.pageY - ofsty });
								var targt = $(e.target).closest('.parse-file'), i, j;
								if(targt.length && targt[0] !== elmnt[0]) {
									i = targt.index();
									j = holdr.index();
									if(i != j) {
										targt[i>j?'after':'before'](holdr);
									}
								}
								break;
						}
					})
					.on('mouseup', function () {
						if(isdrg) {
							if(isdrg == 2) {
								holdr.replaceWith(elmnt);
								//elmnt.css({ 'position':'relative', 'left':0, 'top':0 });
								elmnt.parent().removeClass('parse-dragging').end().removeClass('parse-dragged').css({ 'position':'relative', 'left':0, 'top':0 });
								if(!container.hasClass('parse-image-container')) { elmnt.css('width', 'auto'); }
								update();
							}
							isdrg = 0;
							initx = false;
							inity = false;
							elmnt = false;
							holdr = false;
						}
					});
			}

			if (options.settings) {
				$('#' + options.settings).find(':input').prop('disabled', true).on('keydown', function (e) {
					if (e.which === 13) {
						e.preventDefault();
					}
				});
			}

			if(!options.disabled) {
				upload = new plupload.Uploader({
					runtimes			: options.runtimes,
					browse_button		: container.find('.parse-browse')[0],
					multipart			: true,
					multipart_params	: options.multipart || {},
					url					: options.url,
					chunk_size			: options.chunksize,
					drop_element		: container.find('.parse-list')[0],
					filters : [
						{
							title : (options.types ? "Files" : (options.images ? "Image files" : "All files")),
							extensions : (options.types ? options.types : (options.images ? "jpg,gif,png" : "*"))
						}
					]
				});

				if(options.settings) {
					container.on('click', '.parse-settings', function () {
						var file = $(this).closest('.parse-file'),
							data = file.data('settings'),
							form = $('#' + options.settings), i;
						form.find(':input').val('');
						if (data) {
							for (i in data) {
								if (data.hasOwnProperty(i)) {
									form.find('[name="'+i+'"]').val(data[i]);
								}
							}
						}
						form
							.find('.close')
								.off('click')
								.click(function (e) {
									e.preventDefault();
									form.hide().find(':input').prop('disabled', true);
								}).end()
							.find('.save')
								.off('click')
								.click(function (e) {
									e.preventDefault();
									var updated = form.find(':input').serializeObject();
									form.find(':input').prop('disabled', true);
									$.ajax({
										type : 'POST',
										url : options.url,
										data : {
											id : file.data('id'),
											settings : JSON.stringify(updated)
										}
									}).always(function () {
										file.data('settings', updated);
										if (updated && updated.name) {
											file.find('.parse-title > span').text(updated.name);
										}
										form.hide();
									});
								}).end()
							.find(':input').prop('disabled', false).end()
							.show();
					});
				}
				container
					.on('click', '.parse-remove', function (e) {
						e.preventDefault();
						var pf = $(e.target).closest('.parse-file'),
							id = pf[0].id;
						pf.remove();
						// може да не пробва ако няма клас
						try {
							upload.stop();
							upload.removeFile(upload.getFile(id));
							upload.start();
						} catch(ignore) { }
						upload.refresh();
						update();
						return false;
					})
					.on('dragover', function () {
						$(this).addClass('parse-hover');
					})
					.on('dragexit drop', function () {
						$(this).removeClass('parse-hover');
					})
					.closest('form')
						.on('submit', function (e) {
							if($(this).find('.parse-uploading:eq(0), .parse-wait:eq(0)').length) {
								alert('Файловете още се прикачат. \nМоля, изчакайте или спрете качването.');
								e.preventDefault();
								return false;
							}
						})
						.on('reset', function (e) {
							$(this).find('.parse-file').remove();
							update();
						});

					upload.bind('PostInit', function(up, params) {
						setTimeout(function () { up.refresh(); }, 100);
					});
					upload.bind('FilesAdded', function(up, files) {
						$.each(files, function (i, v) {
							var cnt = container.children('.parse-list'),
								cur = cnt.children('.parse-file').length;
							if(cur && !options.multiple) {
								var pf = cnt.find('.parse-file'),
									id = pf[0].id;
								pf.remove();
								// може да не пробва ако няма клас
								try {
									if(id) {
										up.removeFile(up.getFile(id));
									}
								} catch(e) { }
								cur = 0;
								update();
							}
							if(container.hasClass('parse-image-container') && URL) {
								try {
									(function (id, data) {
										setTimeout(function () {
											var img = new Image();
											img.onload = function () {
												var cnv1 = document.createElement('CANVAS'),
													cnt1 = cnv1.getContext('2d'),
													cnv2 = document.createElement('CANVAS'),
													cnt2 = cnv2.getContext('2d');
												cnv1.setAttribute('width', img.width);
												cnv1.setAttribute('height', img.height);
												cnt1.drawImage(img, 0, 0);
												cnv2.setAttribute('width', '120');
												cnv2.setAttribute('height', '120');
												cnt2.drawImage(cnv1, Math.max(0, (img.width - img.height) / 2), Math.max(0, (img.height - img.width) / 2), Math.min(img.width, img.height), Math.min(img.width, img.height), 0, 0, 120, 120);
												container
													.find('#' + id).filter('.parse-wait, .parse-uploading')
													.css({ 'backgroundImage' : 'url("' + cnv2.toDataURL("image/png") + '")' });
											};
											img.src = URL.createObjectURL(data);
										}, i * 20);
									}(v.id, v.getNative()));
								} catch(ignore) { }
							}
							cnt.append(
								'<div id="' + files[i].id + '" class="parse-wait parse-file ' + (!options.disabled && options.multiple && options.dnd ? ' parse-draggable ' : '') + (options.images ? 'parse-image' : '') + '">' +
									(!options.disabled && options.multiple && options.dnd ? '<span class="parse-drag"><i class="'+(options.file.drag)+'"></i></span>' : '' ) +
									'<span class="parse-remove '+(options.remove.clss)+'"><i class="'+(options.remove.icon)+'"></i>'+options.remove.html+'</span>' +
									(options.settings ? '<span class="parse-settings"><i class="'+(options.file.settings)+'"></i></span>' : '') +
									'<span class="parse-title" draggable="false"><i class="'+(options.file.wait)+'"></i><span>'+files[i].name+'</span></span>' +
									'<span class="parse-progress"><span class="parse-progress-inner"></span></span>' +
								'</div>'
							);
						});
						setTimeout(function () { $.each(up.files, function (i,v) { if(v && v.id && !document.getElementById(v.id)) { try { up.removeFile(v); update(); } catch(e) { } } }); up.refresh(); up.start(); },100);
					});
					upload.bind('BeforeUpload', function(up, file) {
						//params = parse.settings.multipart_params;
						//params.prefix = params.prefix.split('_')[0] + '_' + file.id;
						$('#' + file.id).removeClass("parse-wait").addClass('parse-uploading');
					});
					upload.bind('UploadProgress', function(up, file) {
						$('#' + file.id).find('.parse-progress-inner').css('width', file.percent + '%');
					});
					upload.bind('FileUploaded', function(a,b,c) {
						// IE9!!!
						c = JSON.parse(c.response.replace(/<\/?pre\>/ig,'').replace(/\r/g,''));
						if(parseInt(c.id,10)) {
							$("#" + b.id)
								.removeClass("parse-uploading parse-wait")
								.addClass('parse-complete')
								.data('hash', c.hash)
								.data('id', c.id)
								.find('.parse-progress-inner').css({ 'width':'100%' }).end()
								.find('.parse-title').attr('href', c.url).end()
								.find('.parse-title > i').attr('class', options.file.done).end()
								.append('<div class="parse-preview">' + c.html + '</div>');
							if(options.images && (c.thumb || c.url)) {
								$("#" + b.id).css({'backgroundImage' : 'url('+ (c.thumb || c.url) + ')'});
							}
							update();
						}
						else {
							$("#" + b.id).remove();
							alert('Грешка при качване:' + "\n\n" + 'Не можахме да съхраним файла.');
						}
						a.refresh();
					});
					upload.bind('Error', function(up, e) {
						if(e.file && e.file.id) {
							$("#" + e.file.id).remove();
						}
						alert('Грешка при качване:' + "\n\n" + e.message);
						up.refresh();
					});

				upload.init();
			}
		});
	};

	$(function () {
		$('body')
			.bind('dragover', function () {
				$('.parse-container').not('.parse-disabled').addClass('parse-target');
			})
			.bind('dragexit drop', function () {
				$('.parse-container').not('.parse-disabled').removeClass('parse-target');
			});
	});

}));
