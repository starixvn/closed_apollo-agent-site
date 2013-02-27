function ViewBallNo(id) {
    for (var i = 0; i < 100; i++) {
        if ('ballNo' + i == id) {
            continue;
        }

        if (document.getElementById('ballNo' + i)) {
            document.getElementById('ballNo' + i).style.display = 'none';
        } else {
            break;
        }
    }

    var display = document.getElementById(id).style.display == 'none';
    document.getElementById(id).style.left = '190px';
    document.getElementById(id).style.display = display ? 'block' : 'none';
}