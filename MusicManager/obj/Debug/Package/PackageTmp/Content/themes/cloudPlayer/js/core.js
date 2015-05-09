////ajaxified pages
//$(function () {
//    String.prototype.decodeHTML = function () {
//        return $("<div>", { html: "" + this }).html();
//    };
//    var $main = $("main"),
//    init = function () {
//        // Do this when a page loads.
//    },
//    ajaxLoad = function (html) {
//        document.title = html.match(/<title>(.*?)<\/title>/)[1].trim().decodeHTML()
//        init();
//    },
//    loadPage = function (url) {
//        history.pushState({}, '', href);
//        $main.load(href + " main>*", ajaxLoad);
//    };
//    init();
//    $(window).on("popstate", function (e) {
//        if (e.orginalEvent.state !== null) {
//            loadPage(location.href);
//        }
//    });
//    $(document).on("click", "a, area", function () {
//        var href = $("this").attr("href");
//        if (href.indexOf(document.domain) > -1 || href.indexOf(':') === -1) {
//            loadPage(href);
//            return false;
//        }
//    });
//});
//// THIS IS WHERE THE MAGIC HAPPENS
//$(function () {
//    $('nav a').click(function (e) {
//        $("#loading").show();
//        href = $(this).attr("href");
//        loadContent(href);
//        // HISTORY.PUSHSTATE
//        history.pushState('', 'New URL: ' + href, href);
//        e.preventDefault();
//    });
//    // THIS EVENT MAKES SURE THAT THE BACK/FORWARD BUTTONS WORK AS WELL
//    window.onpopstate = function (event) {
//        $("#loading").show();
//        console.log("pathname: " + location.pathname);
//        loadContent(location.pathname);
//    };
//});
//function loadContent(url) {
//    // USES JQUERY TO LOAD THE CONTENT
//    $.getJSON("content.php", { cid: url, format: 'json' }, function (json) {
//        // THIS LOOP PUTS ALL THE CONTENT INTO THE RIGHT PLACES
//        $.each(json, function (key, value) {
//            $(key).html(value);
//        });
//        $("#loading").hide();
//    });
//    // THESE TWO LINES JUST MAKE SURE THAT THE NAV BAR REFLECTS THE CURRENT URL
//    $('li').removeClass('current');
//    $('a[href="' + url + '"]').parent().addClass('current');
//}
progresBar = $('.jp-progress');
var mainMenu = (function () {

    /*  private space */

    return {

        /*  public space */
            clonesContextInit: function () {
        
            $.contextMenu({
                selector: '.clones [data-toggle=context]',
                autoHide: true,
                delay: 300,
                build: function ($trigger, e) {
   
                    var local_obj = $('#' + $trigger.context.id + '');
                    //local_obj.toggleClass("scaleDown")
    
     
                },

                items: {
                    "add": {
                        name: "Add to playlist",
                        callback: function (key, options) {
                            var local_obj = $('#' + options.$trigger.context.id + '');
                            var local_obj_id_string = options.$trigger.context.id;

                     //clear memory from preview cycle             
                            delete mainViewInit.tilesInfo();
                            console.info(local_obj + "  was added");


                            //add item      
                            local_obj.clone().appendTo("#sortable");

                            //add audio to virtual list
                            var song_data = local_obj.children('.music-play').attr('data-path');
                            var artist = local_obj.children('.music-info').children('.music-artist').text();
                            var song_title = local_obj.children('.music-info').children('.music-song:not(small)').text();

                            myPlaylist.add({
                                title: "" + song_title + "",
                                artist: "" + artist + "",
                                mp3: "" + song_data + "",
                            });

                            //reinitialize dropArea after change has occurred 
                            dropArea.drag({
                                align: "left",
                            });

                            clones.drag({
                                align: "left",
                                dragClone: true,
                                enableCrossDrop: false
                            });

                            //reallocate new objects        
                            new mainViewInit.tilesInfo();
                            new check.checkIfEmpty();


                        }
                    },
                    "play": {
                        name: "Play Song Now",
                        callback: function (key, options) {
                            var local_obj = $('#' + options.$trigger.context.id + '');
                            var local_obj_id_string = options.$trigger.context.id;

                            /*get trigger trigger object*/
                            console.clear();
                            console.info("Data file: " + local_obj.children('.music-play').attr('data-path'));
                            console.info("Artist: " + local_obj.children('.music-info').children('.music-artist').text());
                            console.info("Song: " + local_obj.children('.music-info').children('.music-song:not(small)').text());
                            console.info("Genre: " + local_obj.children('.music-info').children('.music-genre').text());



                            //get item index from real list to push to the virtual list object;
                            var item_index = local_obj.index();
                            var text = local_obj.text();

                            //debug
                            console.log(item_index);

                            //push item to virtual list
                            myPlaylist.play(item_index);
                            progresBar.css({'margin-top':'0px'});
                            // currentItemCursor
                            mainMenu.getCurrentActiveItem(local_obj);

                        }
                    },
                    "copyLink": {
                        name: "Copy Song URL",
                        callback: function (key, options) {
                            var local_obj = $('#' + options.$trigger.context.id + '');
                            var local_obj_id_string = options.$trigger.context.id;


                        }
                    }                                          
                }
            });
    },

        dropareaContextInit: function () {
        

            $.contextMenu({
                selector: '.droparea [data-toggle=context]',
                autoHide: true,
                delay: 300,
                build: function ($trigger, e) {

                 //    if(e.target.parentNode.parentNode.id == "sortable"){ 
                  //   console.info(e.target.parentNode.parentNode);

                    var local_obj = $('#' + $trigger.context.id + '');
                    local_obj.toggleClass("scaleDown")
                             .addClass("context-menu-active");

                    if (local_obj.hasClass('_selected')) {

                        return {
                            callback: function () {},
                            items: {
                                select: {
                                    name: "Unselect Song",
                                    callback: function () {
                                        local_obj.removeClass('_selected');
                                    }
                                }
                            }
                        }
                    }
                    if ($('._selected').length > 1) {

                        return {
                            callback: function () {},
                            items: {
                                remove: {
                                    name: "Remove Songs",
                                    callback: function () {
                                        $('[data-toggle=context]._selected').empty().remove();
                                        dropArea.drag({
                                            align: "left",
                                        });
                                        check.checkIfEmpty();
                                    }
                                },
                                select: {
                                    name: "Unselect Songs",
                                    callback: function () {
                                        $('[data-toggle=context]._selected').removeClass('_selected');
                                        dropArea.drag({
                                            align: "left",
                                        });
                                        check.checkIfEmpty();
                                    }
                                }
                            }
                        }
                    }
           
                },

                items: {
                    "play": {
                        name: "Play Song Now",
                        callback: function (key, options) {
                            var local_obj = $('#' + options.$trigger.context.id + '');
                            var local_obj_id_string = options.$trigger.context.id;

                            /*get trigger trigger object*/
                            console.clear();
                            console.info("Data file: " + local_obj.children('.music-play').attr('data-path'));
                            console.info("Artist: " + local_obj.children('.music-info').children('.music-artist').text());
                            console.info("Song: " + local_obj.children('.music-info').children('.music-song:not(small)').text());
                            console.info("Genre: " + local_obj.children('.music-info').children('.music-genre').text());



                            //get item index from real list to push to the virtual list object;
                            var item_index = local_obj.index();
                            var text = local_obj.text();

                            //debug
                            console.log(item_index);

                            //push item to virtual list
                            myPlaylist.play(item_index);
                            progresBar.css({'margin-top':'0px'});
                            // currentItemCursor
                            mainMenu.getCurrentActiveItem(local_obj);

                        }
                    },
                    "playNext": {
                        name: "Play Song Next",
                        callback: function (key, options) {
                            var local_obj = $('#' + options.$trigger.context.id + '');

                            var song_data = local_obj.children('.music-play').attr('data-path');
                            var song_title = local_obj.children('.music-info').children('.music-song:not(small)').text();
                            var artist = local_obj.children('.music-info').children('.music-artist').text();

                            myPlaylist.add({
                                title: "" + song_title + "",
                                artist: "" + artist + "",
                                free: true,
                                mp3: "" + song_data + "",
                            });
                            mainMenu.getSongPositionInRealList(local_obj_id_string);

                        }
                    },
                    "playLast": {
                        name: "Play Song Last",
                        callback: function (key, options) {
                            var local_obj = $('#' + options.$trigger.context.id + '');

                            $('#' + options.$trigger.context.id + '').children('.music-play').attr('data-path');

                        }
                    },
                    "sep1": "---------",
                    "share": {
                        name: "Share Song",
                        callback: function () {
                            $('#shareModal').modal({})
                        }
                    },
                    "copyLink": {
                        name: "Copy Song URL",
                        callback: function (key, options) {
                            var local_obj = $('#' + options.$trigger.context.id + '');
                            var local_obj_id_string = options.$trigger.context.id;


                        }
                    },     
                    "select": {
                        name: "Select Song",
                        callback: function (key, options) {
                            var local_obj = $('#' + options.$trigger.context.id + '');
                            local_obj.addClass('_selected');

                        }
                    },
                    "addToPlaylist": {
                        name: "Add to Playlist",
                        callback: function (key, options) {
                            var local_obj = $('#' + options.$trigger.context.id + '');
                            local_obj.addClass('_selected');

                        }

                    },
                    "sep2": "---------",
                    "remove": {
                        name: "Remove Song",
                        callback: function (key, options) {
                            var local_obj = $('#' + options.$trigger.context.id + '');

                            //get item index from real list to push to the virtual list object;
                            var item_index = local_obj.index(this.parentNode);

                            //remove item from virtual list
                            myPlaylist.remove(item_index);


                            //elem.parentElement.removeChild(elem);
                            local_obj.empty().remove();
                            dropArea.drag({
                                align: "left",
                            });
                            check.checkIfEmpty();
                        }
                    },

                }
            });


         ///   $('.clones [data-toggle=context]').contextMenu(false);

        },
        quickSearch: function () {


            $("#quick-filter").keyup(function () {

                if ($(this).val() != "") {

                    $('body').find('.droparea > [data-toggle=context]').hide();

                    $('body').find(".droparea > [data-toggle=context]:contains-ci('" + $(this).val() + "')").show();
                    dropArea.drag({
                        align: "left",
                    });

                } else {
                    // When there is no input or clean again, show everything back
                    $('body').find('.droparea > [data-toggle=context]').show();
                    dropArea.drag({
                        align: "left",
                    });

                }
            });

            // jQuery expression for case-insensitive filter
            $.extend($.expr[":"], {
                "contains-ci": function (elem, i, match, array) {
                    return (elem.textContent || elem.innerText || $(elem).text() || "").toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0;
                }
            });



        },
        getCurrentSongDuration: function () {
            $("#player1").bind($.jPlayer.event.timeupdate, function (e) {
                var totalTime_s = e.jPlayer.status.duration;

                var totalTime_m = Math.floor(totalTime_s / 60);
                var totalTime_s = totalTime_s - totalTime_m * 60;
                console.log("Total time:" + totalTime_m + " : " + totalTime_s);
                console.log("Total time:" + totalTime_m + " : " + totalTime_s);


            });

        },
        getSongsInVirtualPlaylist: function () {
            var elements = $('ul#songs li');
            var list = [];
            for (var i = 0; i < elements.length; i++) {
                var element = elements[i];
                var pushed = element.innerText.replace(element.innerText.charAt(0), "").replace('(mp3)', "");
                list.push(pushed);
                console.log(list[i].split(' by '));
                console.log('---');

            }
        },
        getSongsInRealPlaylist: function () {
            var elements = $('div#sortable.droparea div.music-item-container');
            var list = [];


            for (var i = 0; i < elements.length; i++) {
                var element = elements[i];

                list.push(element.innerText.replace(/\s+/g, ' '));

                console.log(list[i].split(' by '));
                console.log('---');



            }
        },
        getSongPositionInRealList: function (id) {
            listItem = $('div#sortable.droparea');
            console.log($("#" + id + ".music-item-container").index(listItem));
            console.log("#" + id + ".music-item-container");
            //myPlaylist.play(2); // Option 1 : Plays the 3rd item
        },
        getCurrentActiveItem: function (object) {
            var current = myPlaylist.current;
            playlist = myPlaylist.playlist;

            $.each(playlist, function (item_index) {
                if (item_index == current) {
                    //  item_index.toggleClass('_selected2');
                } else {
                    $('.droparea .music-item-container').not(object).removeClass('_selected2');
                }

            });
        }, //makes a visual selection of the currently playing song
        pushToVirtualList: function () {
            var elements = $('div#sortable.droparea div.music-item-container');
            var localOBJ = {
                artist: "SPECIAL ARTIST",
                mp3: "songs/_special_song_Touch And Go - Straight To Number One.mp3",
                title: "SPECIAL SONG"
            };
            var show = [];
            for (var i = 0; i < elements.length; i++) {}
            show.push(localOBJ);
            myPlaylist.add(show[0]);
        },
        contructListFromDOM: function (div) {
            myPlaylist.remove();
            var localOBJ = {
                artist: i,
                mp3: i,
                title: i
            };

            var mp3 = div.children('.music-play');
            var song_title = div.children('.music-info').children('.music-song');
            var artist = div.children('.music-info').children('.music-artist');

            for (var i = 0; i < div.length; i++) {
                localOBJ[i] = {
                    artist: artist[i].innerText,
                    mp3: mp3[i].getAttribute('data-path'),
                    title: song_title[i].innerText
                }
                myPlaylist.add(localOBJ[i]);
            }

        },
        checkForCurrentSong: function () {
            $("#player1").bind(jQuery.jPlayer.event.play, function (event) {
                var current = myPlaylist.current,
                    playlist = myPlaylist.playlist;
                $.each(playlist, function (index, obj) {
                    if (index == current) {
                        console.log(obj.title);
                        $('body').find(".droparea > [data-toggle=context]:contains-ci('" + obj.title + "')").addClass('_currentlyPlaying');

                    } else {
                        // When there is no input or clean again, show everything back
                        $('body').find(".droparea > [data-toggle=context]:contains-ci('" + obj.title + "')").removeClass('_currentlyPlaying');

                    }
                });
                event.stopPropagation();
            });
            return event.title;
        },
        pagination: function (arg) {
            //var page = new Array();
            //page = []
            //pageMax = 24;
            //for (i = 0; i < pageMax; i++) {
            //    page[i]
            // }
            var pageMax = 12;


            var div = $(".droparea > [data-toggle=context]");

            var localOBJ = {
                artist: i,
                mp3: i,
                title: i
            };

            var mp3 = div.children('.music-play');
            var song_title = div.children('.music-info').children('.music-song');
            var artist = div.children('.music-info').children('.music-artist');

            for (var i = 0; i < div.length; i++) {
                var pos = arg * pageMax + i + 1;

                localOBJ[i] = {
                    artist: artist[i].innerText,
                    mp3: mp3[i].getAttribute('data-path'),
                    title: song_title[i].innerText,
                    sort_position: pos
                }

            }

            console.info(localOBJ);
        }

    }
})();

//var playlist_object = {

//    myPlaylist = new jPlayerPlaylist({
//        jPlayer: "#player1",
//        cssSelectorAncestor: "#player"
//    }, [
//         {
//             title: "Some Unholy War",
//             artist: "Amy Winehouse",
//             mp3: "songs/Amy Winehouse - Some Unholy War.mp3"
//         },
//    ], {
//        enableRemoveControls: false,
//        swfPath: "js",
//        supplied: "mp3",
//        smoothPlayBar: true,
//        keyEnabled: true,
//        audioFullScreen: false
//    });

//};



$(document).ready(function () {

    myPlaylist = new jPlayerPlaylist({
        jPlayer: "#player1",
        cssSelectorAncestor: "#player"
    }, [{
        title: "Some Unholy War",
        artist: "Amy Winehouse",
        mp3: "songs/Amy Winehouse - Some Unholy War.mp3"
    }, {
        title: "Stronger than Me",
        artist: "Amy Winehouse",
        mp3: "songs/Amy Winehouse - Stronger than Me.mp3"
    }, {
        title: "Tears Dry On Their Own",
        artist: "Amy Winehouse",
        mp3: "songs/Amy Winehouse - Tears Dry On Their Own.mp3"
    }, {
        title: "Wake Up Alone",
        artist: "Amy Winehouse",
        mp3: "songs/Amy Winehouse - Wake Up Alone.mp3"
    }, {
        title: "Deep blue",
        artist: "Antony Raijekov",
        mp3: "songs/Antony Raijekov - Deep blue (2005).mp3"
    }, {
        title: "Don-ki-Not",
        artist: "Antony Raijekov",
        mp3: "songs/Antony Raijekov - Don-ki-Not (2003).mp3"
    }, {
        title: "Drop of whisper",
        artist: "Antony Raijekov",
        mp3: "songs/Antony Raijekov - Drop of whisper (2005).mp3"
    }, {
        title: "EXIT 65",
        artist: "Antony Raijekov",
        mp3: "songs/Antony Raijekov - EXIT 65 (2005).mp3"
    }, {
        title: "Fidder",
        artist: "Antony Raijekov",
        mp3: "songs/Antony Raijekov - Fidder (2004).mp3"
    }, {
        title: "Go 'n' Drop",
        artist: "Antony Raijekov",
        mp3: "songs/Antony Raijekov - Go 'n' Drop (2003).mp3"
    }, {
        title: "Lightout",
        artist: "Antony Raijekov",
        mp3: "songs/Antony Raijekov - Lightout (2003).mp3"
    }, {
        title: "I Get a Kick Out of You",
        artist: "Frank Sinatra",
        mp3: "songs/Frank Sinatra - I Get a Kick Out of You.mp3"
    }, ], {
        enableRemoveControls: false,
        swfPath: "js",
        supplied: "mp3",
        smoothPlayBar: true,
        keyEnabled: true,
        audioFullScreen: false
    });


    var playlistContainer = $('[data-type*="expanded-list"]');
    playlistContainer.jScrollPane();




    dropArea = $(".droparea");
    clones = $(".clones");
    albumItem = $('album');
    var documentH = $(document).height();
    var documentW = $(document).width();

    new navigationInit.viewtype('auto', documentW);

    new mainViewInit.musicTiles(documentW);
    new mainViewInit.tilesInfo();

    new navigationInit.listOverflow();
    new navigationInit.search();
    new navigationInit.shuffle();
    new navigationInit.shuffle();
    new check.checkIfEmpty();

    mainMenu.dropareaContextInit()
    mainMenu.clonesContextInit()

    new mlPushMenu(document.getElementById('mp-menu'), document.getElementById('trigger'));

    new mainMenu.checkForCurrentSong();
    var counter = 0;

    $('#playlistTrigger').on('click', function (e) {
        counter++;
        if (counter === 1) {
            a = true;
            new mainViewInit.setViewState(a);
        } else {
            a = false;
            new mainViewInit.setViewState(a);
            counter = 0;
        }
        e.preventDefault();
    });



    dropArea.drag({
        align: "left",
    });

    clones.drag({
        align: "left",
        dragClone: true,
        enableCrossDrop: false
    });
    new mainMenu.quickSearch();
    //addBulkToPlaylist
    albumItem.on('click', '.album-addTo-currentPlaylist', function (e) {
        delete mainViewInit.tilesInfo();
        console.log($(this).parent() + "  album was added");
        $('album#' + $(this).context.parentNode.id + '').children('.clones').children().clone().removeClass('context-menu-disabled').appendTo("#sortable");
        console.info($(this).context.parentNode.id)
        dropArea.drag({
            align: "left",
        });

        clones.drag({
            align: "left",
            dragClone: true,
            enableCrossDrop: false
        });
        new mainViewInit.tilesInfo();
        // $(this).parent().remove();

        //reallocate new objects        
        new mainViewInit.tilesInfo();
        new check.checkIfEmpty();
        mainMenu.init();
        e.preventDefault();
    });

    //addToCurrentPlaylist
    clones.on('click', '.music-addTo-currentPlaylist', function (e) {


        //clear memory from preview cycle             
        delete mainViewInit.tilesInfo();
        console.log($(this).parent() + "  was added");


        //add item      
        $(this).parent().clone().removeClass('context-menu-disabled').appendTo("#sortable");
        $(this).removeClass('music-addTo-currentPlaylist');
        $(this).html('<i class="fa fa-check-circle"></i>');
        //add audio to virtual list
        var local_obj = $('#' + $(this).parent().attr('id') + '');
        var song_data = local_obj.children('.music-play').attr('data-path');
        var artist = local_obj.children('.music-info').children('.music-artist').text();
        var song_title = local_obj.children('.music-info').children('.music-song:not(small)').text();

        myPlaylist.add({
            title: "" + song_title + "",
            artist: "" + artist + "",
            mp3: "" + song_data + "",
        });

        //reinitialize dropArea after change has occurred 
        dropArea.drag({
            align: "left",
        });

        clones.drag({
            align: "left",
            dragClone: true,
            enableCrossDrop: false
        });

        //reallocate new objects        
        new mainViewInit.tilesInfo();
        new check.checkIfEmpty();


        e.preventDefault();
    }); //!addToCurrentPlaylist 


    $(".nav").on('click', '[navbar-control=play]', function () {
        $(this).attr('navbar-control', 'pause');
        $(this).children('i').removeClass('fa-play').addClass('fa-pause');
        $('#player1').jPlayer("play");
        $("#player1").bind($.jPlayer.event.timeupdate, function (event) {

            var currentTime = event.jPlayer.status.currentTime;
            var minutes = Math.floor(currentTime / 60);
            var seconds = currentTime - minutes * 60;
            var formatted_s = "0" + seconds;


            if (seconds < 10) {
                console.log(minutes + " : " + formatted_s);
            } else console.log(minutes + " : " + seconds);

        });
        progresBar.css({'margin-top':'0px'});
        //current duration
        mainMenu.getCurrentSongDuration();

    });
    $(".nav").on('click', '[navbar-control=pause]', function () {
        $("#player1").unbind($.jPlayer.event.timeupdate);
        $(this).attr('navbar-control', 'play');
        $(this).children('i').removeClass('fa-pause').addClass('fa-play');
        $('#player1').jPlayer("pause");


    });
    $(".nav").on('click', '[navbar-control=stop]', function () {
        $('[navbar-control=pause]').attr('navbar-control', 'play');
        $('[navbar-control=play]').children('i').removeClass('pause').addClass('play');
        progresBar.css({'margin-top':'-5px'});
        $('#player1').jPlayer("stop");
    });
    $(".nav").on('click', '[navbar-control=forward]', function () {
        $("#player1").unbind($.jPlayer.event.timeupdate);
        //current duration
        mainMenu.getCurrentSongDuration();

        //action
        myPlaylist.next();

        // currentItemCursor
        mainMenu.getCurrentActiveItem();

    });
    $(".nav").on('click', '[navbar-control=backward]', function () {
        $("#player1").unbind($.jPlayer.event.timeupdate);
        myPlaylist.previous();
        //current duration
        mainMenu.getCurrentSongDuration();

        // currentItemCursor
        mainMenu.getCurrentActiveItem();

    });

    //removal of items
    dropArea.on('click', '.music-removeFrom-currentPlaylist', function (e) {


        //reallocate new objects
        new mainViewInit.tilesInfo();
        new check.checkIfEmpty();

        //get item index from real list to push to the virtual list object;
        var item_index = $(this).parent().index(dropArea);
        console.info("doesn't work yet");
        //remove item from virtual list
        myPlaylist.remove(item_index);



        //remove item
        $(this).parent().empty().remove();

        navigationInit.viewtype('auto', documentW);
        console.log($(this).parent() + "  was removed");


        //reinitialize dropArea after change has occurred   
        dropArea.drag({
            align: "left",

        });

        clones.drag({
            align: "left",
            dragClone: true,
            enableCrossDrop: false
        });


        //prevent default
        e.preventDefault();

    }); //!removal of items

    //resizeEvents
    $(window).bind('resize', function () {
        var documentH = $(document).height();
        var documentW = $(document).width();

        mainViewInit.musicTiles(documentW);
        navigationInit.listOverflow();
        check.checkIfEmpty();
        mainViewInit.tilesInfo();
        navigationInit.viewtype('auto', documentW);

    });


    $('.parallax-layer').parallax();

    //dynamic view
    //var lalala = $('.music-action.music-artistProfile');
    //dropArea.on('click', lalala, function (e) {

    //    //console.log('test');
    //    viewIsDynamic = new Boolean;
    //    viewIsDynamic = true;
    //    var mainView = $('#mainViewQueue');
    //    var dynamicContainer = $('#dynamicContainer');
    //    var destroyView = $('#destroyView');

    //    mainView.css({ 'transform': 'translate3d(0,100%,0)', 'display': 'none' });
    //    dynamicContainer.css({ 'transform': 'translate3d(0, 0,0)' });

    //    if (viewIsDynamic) {
    //        destroyView.on('click', function () {
    //            console.log('Back to music queue');
    //            mainView.css({ 'transform': 'translate3d(0,0,0)', 'display': 'block' });
    //            dynamicContainer.css({ 'transform': 'translate3d(0,0,0)' });
    //        });
    //        //reinitialize dropArea after change has occurred     
    //        dropArea.html(dropArea.html());
    //        dropArea.drag({
    //            align: "left"
    //        });

    //    }


    //    e.stopPropagation();
    //});

}); // end document ready

function itemAlert() {};

function select(y) {
    var state = new Boolean;
    state = true;
    $('#' + y.currentTarget.id + '').toggleClass('_selected');
    $('.droparea .music-item-container').each(function () {

    });
    console.log(state);
    return state;

}

function unSelect(w) {
    if ($('#' + w.target.id + '').hasClass('_selected')) {
        $('#' + w.target.id + '').removeClass('_selected');
    }
}

function remove(x) {

    x.target.remove();
    check.checkIfEmpty();
    dropArea.drag({
        align: "left",
    });
}

function removeAll(z) {
    console.info(z.target.id);
    if ($('.droparea .music-item-container').hasClass('_selected')) {
        $('.droparea .music-item-container._selected').remove();
        check.checkIfEmpty();
        dropArea.drag({
            align: "left",
        });
    } else {
        alert('Nothing to remove');
    }
}
var mainViewInit = (function () {

    /*  private space */

    return {

        /*  public space */

        musicTiles: function (a) {

            var maxTileSize = a / 7;
            for (var i = 0; i < $('.droparea .music-item-container').length; i++); {
                //console.log(dimensionW +"and");
                $('.music-item-container').css({
                    "width": maxTileSize,
                    "height": maxTileSize
                });

            }

            dropArea.html(dropArea.html());
            clones.html(clones.html());

            dropArea.drag({
                align: "left",
            });

            clones.drag({
                align: "left",
                dragClone: true,
                enableCrossDrop: false
            });

            // init($(this).id);


            //$('.droparea .music-item-container').each(function () {

            //    idString = $(this).attr("id");
            //    _this = $(this);
            //    console.log(idString);
            //    //console.info(_this);
            //    mainViewInit.mainMenu(idString, _this);
            //});

            return maxTileSize;
        },
        setViewState: function (state) {
            var mainView = $('#firstHalf');
            var playlistView = $('#secondHalf');
            if (state) {
                mainView.css({
                    'transform': 'translate3d(-100%,0,0)',
                    '_display': 'none'
                });
                playlistView.css({
                    'transform': 'translate3d(0, 0,0)',
                    '_display': 'block'
                });
                delete mainViewInit.tilesInfo();
                //console.log('switch to playlist');
            } else {
                mainView.css({
                    'transform': 'translate3d(0,0,0)',
                    '_display': 'block'
                });
                playlistView.css({
                    'transform': 'translate3d(100%, 0,0)',
                    '_display': 'none'
                });
                //console.log('switch to main');
            }
        },
        tilesInfo: function () {
            var musicItemContainer = $('.music-item-container');

            var _options, _opPositions = new Array;
            _item = $('.music-action');
            _options = ['.music-addTo-currentPlaylist', '.music-removeFrom-currentPlaylist', '.music-love', '.music-artistProfile'];
            _opPositions = ['0px', '0px', _item.height(), _item.height() * 2];

            musicItemContainer.mouseenter(function (event) {
                for (counter = 0; counter < _options.length; counter++) {

                    $(this).children(_options[counter]).css({
                        'transform': 'translate3d(0,0,0)',
                        'transition': 'all 0.' + counter + 's ease',
                        'bottom': _opPositions[counter]
                    });

                }
                event.stopPropagation();
            });
            musicItemContainer.mouseleave(function (event) {

                for (counter = 0; counter < _options.length; counter++) {

                    $(this).children(_options[counter]).css({
                        'transform': 'translate3d(0,100%,0)',
                        'transition': 'all 0.' + counter + 's ease',
                        'bottom': '0px'
                    });

                }
                event.stopPropagation();

            });




        }
    }
})();

var navigationInit = (function () {

    /*
        private space
    */

    return {
        changeViewDirective : function (view_value, view_title, view_type){

            var backViewButton = $('#backViewButton');
            var viewSelector = $('#firstHalf');
            var subTitle = "audiostem cloud player";
            var mainViewQueue = $('#mainViewQueue[view-type="container"]');
            var dynamicContainer = $('#dynamicContainer[view-type="container"]');
            var viewType = '[view-type="'+ view_type +'"]';
        

           if(view_value){
                viewSelector.addClass('no-transparency-page');
                mainViewQueue.css({'visibility':'hidden'});
                dynamicContainer.css({'_transform':'translate3d(0,-100%,0)', 'display':'block'});
                backViewButton.css({'width': '50px'})
                              .on('click', function (){
                                   navigationInit.changeViewDirective(false);
                              });

                if(view_type === 'artist'){            
                    document.title = "\u25BA " + view_title + "'s Page | " + subTitle;
                }if(view_type === 'venue'){
                    document.title = view_title + " | " + subTitle;
                }if(view_type === 'album' || view_type === 'song'){
                    document.title = "\u25BA " + view_title + " | " + subTitle;
                }



           }
           else{
                viewSelector.removeClass('no-transparency-page');
                mainViewQueue.css({'visibility':'visible'});
                dynamicContainer.css({'_transform':'translate3d(0,0,0)', 'display':'none'}); //remember to add .empty(), because in a a server medium, we always get fresh content

                backViewButton.css({'width': '0px'})
                              .on('click', function (){
                                    navigationInit.changeViewDirective(true);
                              });  
                 document.title = "audiostem cloud player";                            

           }


        },

        listOverflow: function () {
            var documentH = $(document).height();
            var listItem = $('ul[data-type*="expanded-list"]');
            var mainMenu = $('.mp-menu, .mp-pusher');
            //console.log(listItem.length);
            listItem.css({
                'height': documentH - 173,
                'overflow-y': 'auto'
            });
            mainMenu.css({
                'height': documentH
            });
            listItem.jScrollPane();
        },
        viewtype: function (height, width) {

            var containerSelector = $('#mainViewQueue[view-type*="container"]');

            containerSelector.css({
                'width': width - 100,
                'height': height
            });

            containerSelector === null;
        },
        search: function (a) {
            var showSearch = $('[navbar-control="search"]');
            var searchContainer = $('#navbar-search');

            showSearch.on('click', function () {
                searchContainer.toggleClass('search-container-focus');
            });
            showSearch, searchContainer === null;
        },
        shuffle: function (b) {
            delete mainViewInit.tilesInfo();

            var shuffleButton = $('[navbar-control="shuffle"]');

            shuffleButton.on('click', function (e) {
                $("#sortable").trigger("shuffle");
                mainMenu.contructListFromDOM($('#sortable div.music-item-container'));
                new mainViewInit.tilesInfo();
                e.preventDefault();
            });
            shuffleButton === null;

        }
    }
})(); 


var check = (function () {

    /*
        private space
    */

    return {
        checkIfEmpty: function () {

            var emptyState = new Boolean;
            var songTotalContainer = $('.songTotal');
            var notif = $('.droparea #emptyNotification');
            item = $('.droparea .music-item-container');

            songTotalStringValue = item.length;


            songTotalContainer.html(songTotalStringValue);
            console.log("Total songs in playlist: " + songTotalStringValue);

            //reallocate new objects
            new mainViewInit.tilesInfo();

            if (item.length < 1)

            {

                //itemContainer.html('<h4 id="emptyNotification">The playlist is currently empty</h4>');
                // dropArea.css('height', 'auto');
                emptyState = true;
                console.log('List is empty');

            } else {
                emptyState = false;
                notif.remove();

                notif = null;
                console.log('List is not empty');
            }


            return emptyState;

        },
        forceEmptyAndNew: function () {
            if (!check.checkIfEmpty()) {
                item.remove();
                dropArea.empty();
                check.checkIfEmpty();
                myPlaylist.remove();
            } else {
                console.log('List is already empty')

            }
        }
    }
})();


var setAttributes = function(element, attributes){
    for(attribute in attributes){
        element[attribute] = attributes[attribute];
        }
}
var themeChanger = (function(){

    return {
        enableTheme: function(id, themeName){
            var style = document.createElement("link");
            setAttributes(style, {
                rel : "stylesheet",
                type : "text/css",
                id : id,
                href: "themes/"+themeName+".css"
            });
            document.getElementsByTagName("head")[0].appendChild(style);
        },

        disableTheme: function(id){
            var style = document.getElementById(id);
            if(style){
                style.parentNode.removeChild(style);
            }
        },
            
        swapStyleSheet: function(id, url){
            this.removeStyleSheet(id);
            this.addStyleSheet(id, url);
        }

    }
})();


var saveUserPreferences = (function(){


return{
    //saveThemeOption();

}
})();