
$| = 1;

foreach my $curFileName (<nobu15pk_*.png>) { # ★nobu16起点の場合。なったらここ書き換えること
    print STDERR "$curFileName\n";
    (my $number) = $curFileName =~ m/_(\d+)\.png/;
	if ($number >= 0) {
		foreach my $targetFileName (<*.png>) {
	        if ($curFileName eq $targetFileName) {
				next;
			}
			my $ret = `KaoORBMatching $curFileName $targetFileName`;
			if ($ret =~ /一致：/ ) {
				print "$curFileName=>$targetFileName\n";
				print "$targetFileName=>$curFileName\n";
			}
		}
	}

}
