
$| = 1;

foreach my $curFileName (<nobu15pk_*.png>) { # ��nobu16�N�_�̏ꍇ�B�Ȃ����炱�����������邱��
    print STDERR "$curFileName\n";
    (my $number) = $curFileName =~ m/_(\d+)\.png/;
	if ($number >= 0) {
		foreach my $targetFileName (<*.png>) {
	        if ($curFileName eq $targetFileName) {
				next;
			}
			my $ret = `KaoORBMatching $curFileName $targetFileName`;
			if ($ret =~ /��v�F/ ) {
				print "$curFileName=>$targetFileName\n";
				print "$targetFileName=>$curFileName\n";
			}
		}
	}

}
