
$| = 1;

foreach my $curFileName (<nobu15pk_*.png>) {
    print STDERR "$curFileName\n";
    (my $number) = $curFileName =~ m/_(\d+)\.png/;
	if ($number >= 0) {
		my $ret = `KaoORBMatching_multi $curFileName`;
		if ($ret =~ /=>/sm ) {
			print $ret;
		}
	}

}
