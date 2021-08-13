/*globals jQuery, define, module, exports, require, window, document, postMessage, config */
(function (factory) {
	"use strict";
	if (typeof define === 'function' && define.amd) {
		define(['jquery', 'plupload/base'], factory);
	}
	else if(typeof module !== 'undefined' && module.exports) {
		module.exports = factory(require('jquery'), require('plupload/base'));
	}
	else {
		factory(jQuery, plupload);
	}
}(function ($, plupload, undefined) {
	"use strict";

	if($.colorin) { return; }

	$.colorin = {};
	$.colorin.defaults = {
		runtimes     : 'html5,html4',
		url          : typeof config === 'object' && config.upload ? config.upload : '',
		chunksize    : typeof config === 'object' && config.chunksize ? config.chunksize : '1mb',
		multipart    : {},
		disabled     : false,
		multiple     : false,
		images       : false,
		download     : false,
		browse       : {
			clss : 'btn btn-info',
			html : 'Избери файлове',
			icon : 'fa fa-plus'
		},
		zip          : {
			clss : 'btn btn-info',
			html : 'Изтегли файлове',
			icon : 'fa fa-archive'
		},
		remove       : {
			clss : '',
			html : '',
			icon : 'fa fa-remove'
		},
		rename      : {
			clss : '',
			html : '',
			icon : 'fa fa-pencil',
			prompt : 'Моля, въведете име на файла'
		},
		file         : {
			done : 'fa fa-file',
			wait : 'fa fa-refresh'
		},
		value        : null,
		dnd          : false,
		changed      : null,
		renamed      : null
	};

	if(typeof config === 'object' && config.token) {
		$.colorin.defaults.multipart	= { "_csrf_token" : config.token };
	}

	$.colorin.create = function (el, settings) {
		if(!settings) { settings = {}; }
		var options = $.extend(true, {}, $.colorin.defaults, settings, ($(el).data('colorin') || {}));
		var uploader = new plupload.Uploader({
				runtimes			: options.runtimes,
				browse_button		: $(el)[0],
				multipart			: true,
				multipart_params	: options.multipart || {},
				url					: options.url,
				chunk_size			: options.chunksize,
				filters : [
					{ title : options.images ? "Image files" : "All files", extensions : options.images ? "jpg,gif,png" : "*"}
				]
			});
		uploader.init();
		return uploader;
	};

	$.fn.colorin = function (settings) {
		return this.each(function () {
				var options     = $.extend(true, {}, $.colorin.defaults, settings, ($(this).data('colorin') || {})),
					field       = $(this),
					value       = $(this).val(),
					container   = null,
					upload      = null, i, j, k, l,
					update      = function () {
						var tmp = [],
							files = [];
						container.find('.colorin-file').each(function () {
							var id = $(this).data('id'), //.toString();
								points = [];
							if(id) {
								id = id.toString();
								tmp.push(id);
								$(this).find('.colorin-point').each(function () {
									var p = $(this);
									points.push(parseInt(p.css('left'),10));
									points.push(parseInt(p.css('top'),10));
									points.push(p.hasClass('colorin-point-alternative')?1:0);
								});
								files.push({
									'id'    : id,
									'url'   : $(this).find('.colorin-title').attr('href'),
									'hash'  : $(this).data('hash'),
									'html'  : $(this).find('.colorin-title > span').text(),
									'thumb' : options.images ? $(this).find('img').attr('src') : '',
									'points': points
								});
							}
							points.unshift(id);
							field.val(points.join(','));
						});
						if(options.changed) {
							options.changed.call(this, options.multiple ? tmp : (tmp[0] || null), options.multiple ? files : (files[0] || null));
						}
					},
					point      = function (par, x, y, alt) {
						par.append('<div class="colorin-point '+(alt?'colorin-point-alternative':'')+'" style="display:block; left:'+(x)+'px; top:'+(y)+'px;"></div>');
					};

			// create container
			container = $(this).parent().find('[type="file"]').hide().end().end()
				.wrap('<div class="colorin-container ' + (options.multiple ? 'colorin-multiple' : 'colorin-single') + ' ' + (options.images ? 'colorin-image-container' : 'colorin-document-container') + (options.disabled ? ' colorin-disabled' : '') + '"></div>').parent()
				.append('<div class="colorin-buttons"></div><div class="colorin-list"></div>')
				.children('.colorin-buttons')
					.append('<a href="#" class="colorin-browse '+options.browse.clss+'"><i class="'+options.browse.icon+'"></i> '+options.browse.html+'</a>').end();
			if(options.multiple && options.download) {
				container.children('.colorin-buttons')
					.append('<a href="' + options.download + '" class="colorin-zip '+options.zip.clss+'"><i class="'+options.zip.icon+'"></i> '+options.zip.html+'</a>');
			}
			if(options.value) {
				if(!$.isArray(options.value)) {
					options.value = [options.value];
				}
				for(i = 0, j = options.value.length; i < j; i++) {
					container.children('.colorin-list').append('' +
						'<div data-id="'+(options.value[i].id || '')+'" data-hash="'+(options.value[i].hash || '')+'" '+(options.images? '' : '')+' class="colorin-complete colorin-file ' + (options.images ? 'colorin-image' : '') + ' '+(options.value[i].clss||'')+'">' +
							'<img src="'+options.value[i].thumb+'" />' +
							'<span class="colorin-remove '+(options.remove.clss)+'"><i class="'+(options.remove.icon)+'"></i>'+options.remove.html+'</span>' +
							(options.renamed ? '<span class="colorin-rename '+(options.rename.clss)+'"><i class="'+(options.rename.icon)+'"></i>'+options.rename.html+'</span>' : '') +
							'<small class="colorin-details">'+(options.value[i].extra ? options.value[i].extra.join('&nbsp;&bull;&nbsp;') : '')+'</small>' +
							'<a class="colorin-title" target="_blank" href="'+(options.value[i].url || '#')+'" draggable="false"><i class="'+(options.value[i].icon||options.file.done)+'"></i><span>'+options.value[i].html+'</span></a>' +
							'<span class="colorin-progress"><span class="colorin-progress-inner" style="width:100%;"></span></span>' +
						'</div>'
					);
					if(options.value[i].points) {
						for(k = 0, l = options.value[i].points.length; k < l; k+=3) {
							point(container.find('.colorin-file').last(), options.value[i].points[k], options.value[i].points[k+1], parseInt(options.value[i].points[k+2],10));
						}
					}
				}
			}
			if(!options.disabled) {
				container
					.on('click', 'img', function (e) {
						e.preventDefault();
						if(e.target.tagName.toLowerCase() === 'img') {
							var img = $(this),
								off = img.offset();
							point(img.parent(), (e.pageX - off.left), (e.pageY - off.top));
						}
						update();
					})
					.on('click', '.colorin-point', function (e) {
						$(this).toggleClass('colorin-point-alternative');
						update();
					})
					.on('dblclick', '.colorin-point', function (e) {
						$(this).remove();
						update();
					});
			}

			if(!options.disabled) {
				upload = new plupload.Uploader({
					runtimes			: options.runtimes,
					browse_button		: container.find('.colorin-browse')[0],
					multipart			: true,
					multipart_params	: options.multipart || {},
					url					: options.url,
					chunk_size			: options.chunksize,
					drop_element		: container.find('.colorin-list')[0],
					filters : [
						{ title : options.images ? "Image files" : "All files", extensions : options.images ? "jpg,gif,png" : "*"}
					]
				});

				if(options.renamed) {
					container.on('click', '.colorin-rename', function () {
						var html = $(this).closest('.colorin-file').find('.colorin-title > span');
						var name = window.prompt(options.rename.prompt, html.text());
						if(name) {
							html.text(name);
							options.renamed.call(this, $(this).closest('.colorin-file').data("id"), name);
						}
					});
				}
				container
					.on('click', '.colorin-remove', function (e) {
						e.preventDefault();
						var pf = $(e.target).closest('.colorin-file'),
							id = pf[0].id;
						pf.remove();
						// може да не пробва ако няма клас
						try {
							upload.stop();
							upload.removeFile(upload.getFile(id));
							upload.start();
						} catch(ignore) { }
						upload.refresh();
						container.find('.colorin-point').remove();
						update();
						return false;
					})
					.on('dragover', function () {
						$(this).addClass('colorin-hover');
					})
					.on('dragexit drop', function () {
						$(this).removeClass('colorin-hover');
					})
					.closest('form')
						.on('submit', function (e) {
							if($(this).find('.colorin-uploading:eq(0), .colorin-wait:eq(0)').length) {
								alert('Файловете още се прикачат. \nМоля, изчакайте или спрете качването.');
								e.preventDefault();
								return false;
							}
						})
						.on('reset', function (e) {
							$(this).find('.colorin-file').remove();
							container.find('.colorin-point').remove();
							update();
						});

					upload.bind('PostInit', function(up, params) {
						setTimeout(function () { up.refresh(); }, 100);
					});
					upload.bind('FilesAdded', function(up, files) {
						$.each(files, function (i, v) {
							var cnt = container.children('.colorin-list'),
								cur = cnt.children('.colorin-file').length;
							if(cur && !options.multiple) {
								var pf = cnt.find('.colorin-file'),
									id = pf[0].id;
								pf.remove();
								// може да не пробва ако няма клас
								try {
									if(id) {
										up.removeFile(up.getFile(id));
									}
								} catch(e) { }
								cur = 0;
								container.find('.colorin-point').remove();
								update();
							}
							if(container.hasClass('colorin-image-container') && URL) {
								try {
									(function (id, data) {
										return;
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
												cnv2.setAttribute('width', '940');
												cnv2.setAttribute('height', '940');
												cnt2.drawImage(cnv1, Math.max(0, (img.width - img.height) / 2), Math.max(0, (img.height - img.width) / 2), Math.min(img.width, img.height), Math.min(img.width, img.height), 0, 0, 940, 940);
												container
													.find('#' + id).filter('.colorin-wait, .colorin-uploading')
													.css({ 'backgroundImage' : 'url("' + cnv2.toDataURL("image/png") + '")' });
											};
											img.src = URL.createObjectURL(data);
										}, i * 20);
									}(v.id, v.getNative()));
								} catch(ignore) { }
							}
							cnt.append(
								'<div id="' + files[i].id + '" class="colorin-wait colorin-file ' + (options.images ? 'colorin-image' : '') + '">' +
									'<img src="#" />' +
									'<span class="colorin-remove '+(options.remove.clss)+'"><i class="'+(options.remove.icon)+'"></i>'+options.remove.html+'</span>' +
									(options.renamed ? '<span class="colorin-rename '+(options.rename.clss)+'"><i class="'+(options.rename.icon)+'"></i>'+options.rename.html+'</span>' : '') +
									'<a class="colorin-title" href="#" target="_blank" draggable="false"><i class="'+(options.file.wait)+'"></i><span>'+files[i].name+'</span></a>' +
									'<span class="colorin-progress"><span class="colorin-progress-inner"></span></span>' +
								'</div>'
							);
						});
						setTimeout(function () { $.each(up.files, function (i,v) { if(v && v.id && !document.getElementById(v.id)) { try { up.removeFile(v); update(); } catch(e) { } } }); up.refresh(); up.start(); },100);
					});
					upload.bind('BeforeUpload', function(up, file) {
						//params = plupload.settings.multipart_params;
						//params.prefix = params.prefix.split('_')[0] + '_' + file.id;
						$('#' + file.id).removeClass("colorin-wait").addClass('colorin-uploading');
					});
					upload.bind('UploadProgress', function(up, file) {
						$('#' + file.id).find('.colorin-progress-inner').css('width', file.percent + '%');
					});
					upload.bind('FileUploaded', function(a,b,c) {
						// IE9!!!
						c = JSON.parse(c.response.replace(/<\/?pre\>/ig,'').replace(/\r/g,''));
						if(parseInt(c.id,10)) {
							$("#" + b.id)
								.removeClass("colorin-uploading colorin-wait")
								.addClass('colorin-complete')
								.data('hash', c.hash)
								.data('id', c.id)
								.find('.colorin-progress-inner').css({ 'width':'100%' }).end()
								.find('.colorin-title').attr('href', c.url).end()
								.find('.colorin-title > i').attr('class', options.file.done);
							if(options.images && (c.thumb || c.url)) {
								$("#" + b.id).find('img').attr('src', (c.thumb || c.url).replace('&h=128','').replace('128', '940'));
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
				$('.colorin-container').not('.colorin-disabled').addClass('colorin-target');
			})
			.bind('dragexit drop', function () {
				$('.colorin-container').not('.colorin-disabled').removeClass('colorin-target');
			});
	});

}));
