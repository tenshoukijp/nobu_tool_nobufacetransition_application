# 分析データもまだ完璧ではない。
# 分析データから、分析しきれなかったが、前後の状況から、より詳細に追加分析する。


my %AlreadyHashData = ();

open FH, "Link.txt";
foreach $line(<FH>) {
	chomp($line);
	my ($prev, $post) = $line =~ /^(.+?)=>(.+?)$/;
	$AlreadyHashData{$line} = 1;
	my $revs = $post.'=>'.$post;
	$AlreadyHashData{$revs} = 1;
}
close FH;




open FH, "Link.txt";

my @lstAllLinkData = <FH>;

close FH;

$| = 1;

foreach my $curFileName (<*.png>) {
	my ($preFilePrefix, $num, $preFileSurfix) = $curFileName =~ /(........_)(\d+)(\.png)/;
	if ($preFilePrefix) {
		$num--;
		$num = sprintf("%04d", $num);
		$preFileName = $preFilePrefix . $num . $preFileSurfix;

		foreach $line(@lstAllLinkData) {
			my ($preLinkPrefix, $linknum, $preLinkSurfix) = $line =~ m/^$preFileName\=\>(nobu...._)(\d+)(\.png)/x;
			if ($preLinkPrefix && $linknum) {
				$linknum++;
				$linknum = sprintf("%04d", $linknum);
				$preLinknFileName = $preLinkPrefix . $linknum . $preLinkSurfix;
				# print $curFileName, " ", $preLinknFileName, "\n";
				my $newTargetName = $curFileName."=>".$preLinknFileName;
				if ($AlreadyHashData{$newTargetName}) {
					next;
				}
				if ($curFileName eq $preLinknFileName) {
					next;
				}
				if (-e $preLinknFileName) {
					my $ret = `KaoORBMatching $curFileName $preLinknFileName`;
					if ($ret =~ /一致：/ ) {
						print "$curFileName=>$preLinknFileName\n";
					}
				}
			}
		}

	}

	my ($pstFilepstfix, $num, $pstFileSurfix) = $curFileName =~ /(........_)(\d+)(\.png)/;
	if ($pstFilepstfix) {
		$num++;
		$num = sprintf("%04d", $num);
		$pstFileName = $pstFilepstfix . $num . $pstFileSurfix;

		foreach $line(@lstAllLinkData) {
			my ($pstLinkpstfix, $linknum, $pstLinkSurfix) = $line =~ m/^$pstFileName\=\>(nobu...._)(\d+)(\.png)/x;
			if ($pstLinkpstfix && $linknum) {
				$linknum--;
				$linknum = sprintf("%04d", $linknum);
				$pstLinknFileName = $pstLinkpstfix . $linknum . $pstLinkSurfix;
				# print $curFileName, " ", $pstLinknFileName, "\n";
				my $newTargetName = $curFileName."=>".$pstLinknFileName;
				if ($AlreadyHashData{$newTargetName}) {
					next;
				}
				if ($curFileName eq $pstLinknFileName) {
					next;
				}
				if (-e $pstLinknFileName) {
					my $ret = `KaoORBMatching $curFileName $pstLinknFileName`;
					if ($ret =~ /一致：/ ) {
						print "$curFileName=>$pstLinknFileName\n";
					}
				}
			}
		}

	}

}


