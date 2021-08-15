window.addEventListener('contextmenu', function (e) { 
    // do something here... 
    this.alert("Right click is disabled")
    e.preventDefault(); 
  }, false);